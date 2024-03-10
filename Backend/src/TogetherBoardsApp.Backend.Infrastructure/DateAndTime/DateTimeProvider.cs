using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;

namespace TogetherBoardsApp.Backend.Infrastructure.DateAndTime;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}