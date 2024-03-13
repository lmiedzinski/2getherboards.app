using TogetherBoardsApp.Backend.Application.Abstractions;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;

public sealed record LogInUserAccountCommand(string Email, string Password)
    :ICommand<LogInUserAccountResponse>;