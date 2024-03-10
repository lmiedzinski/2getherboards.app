namespace TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}