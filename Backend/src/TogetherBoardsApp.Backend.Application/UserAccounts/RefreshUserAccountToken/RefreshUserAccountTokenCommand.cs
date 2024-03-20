using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.śApplication.UserAccounts.RefreshUserAccountToken;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.RefreshUserAccountToken;

public sealed record RefreshUserAccountTokenCommand(string RefreshToken)
    : ICommand<RefreshUserAccountTokenResponse>;