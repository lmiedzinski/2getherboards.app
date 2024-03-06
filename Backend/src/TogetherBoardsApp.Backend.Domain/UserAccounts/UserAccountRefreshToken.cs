namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public record UserAccountRefreshToken(string Value, bool IsActive, DateTime ExpirationDateUtc);