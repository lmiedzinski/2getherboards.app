using MediatR;
using Microsoft.Extensions.Logging;
using TogetherBoardsApp.Backend.Application.Abstractions;

namespace TogetherBoardsApp.Backend.Application.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommandBase
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            _logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Command {Command} processing failed", name);

            throw;
        }
    }
}