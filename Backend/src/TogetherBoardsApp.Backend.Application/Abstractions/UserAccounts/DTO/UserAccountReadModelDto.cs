namespace TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts.DTO;

public sealed class UserAccountReadModelDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Name { get; init; } = null!;
}