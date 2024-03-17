namespace TogetherBoardsApp.Backend.Infrastructure.OutboxPattern;

public sealed class OutboxPatternOptions
{
    public const string SectionName = "OutboxPatternOptions";
    
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}