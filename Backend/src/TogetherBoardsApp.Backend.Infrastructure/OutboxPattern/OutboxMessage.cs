namespace TogetherBoardsApp.Backend.Infrastructure.OutboxPattern;

public sealed class OutboxMessage
{
    public OutboxMessage(Guid id, DateTime createdAtUtc, string type, string content)
    {
        Id = id;
        CreatedAtUtc = createdAtUtc;
        Content = content;
        Type = type;
    }
    
    public Guid Id { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public string Type { get; private set; }
    public string Content { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }
    public string? Error { get; private set; }
}