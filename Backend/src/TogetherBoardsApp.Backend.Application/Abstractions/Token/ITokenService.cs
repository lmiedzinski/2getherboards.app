using TogetherBoardsApp.Backend.Domain.UserAccounts;

namespace TogetherBoardsApp.Backend.Application.Abstractions.Token;

public interface ITokenService
{
    string GenerateAccessToken(UserAccountId userAccountId);
    string GenerateRefreshToken();
    int GetRefreshTokenLifetimeInMinutes();
    UserAccountId GetUserAccountIdFromContext();
}