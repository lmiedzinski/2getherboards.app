namespace TogetherBoardsApp.Backend.Application.UserAccounts.GetCurrentLoggedInUserAccount;

public sealed record GetCurrentLoggedInUserAccountQueryResponse(
    Guid Id,
    string Email,
    string Name);