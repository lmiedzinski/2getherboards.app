using System.Data;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Domain.Abstractions;
using TogetherBoardsApp.Backend.Infrastructure.Database.SqlConnection;

namespace TogetherBoardsApp.Backend.Infrastructure.OutboxPattern;

internal sealed class OutboxPatternBackgroundJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    
    private readonly SqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxPatternOptions _outboxOptions;
    private readonly ILogger<OutboxPatternBackgroundJob> _logger;

    public OutboxPatternBackgroundJob(
        SqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxPatternOptions> outboxOptions,
        ILogger<OutboxPatternBackgroundJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Outbox pattern background job started");
        
        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        
        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);
        
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<DomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                _logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();

        _logger.LogInformation("Outbox pattern background job finished");
    }
    
    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        var sql = $"""                
            SELECT id, content
            FROM outbox_messages
            WHERE processed_at_utc IS NULL
            ORDER BY created_at_utc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
            """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            transaction: transaction);

        return outboxMessages.ToList();
    }
    
    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_at_utc = @ProcessedAtUtc,
                error = @Error
            WHERE id = @Id";

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedAtUtc = _dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }
    
    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}