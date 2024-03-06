namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public record UserAccountId(Guid Value)
{
    public static implicit operator Guid(UserAccountId userAccountId) => userAccountId.Value;
    public static implicit operator UserAccountId(Guid userAccountId) => new(userAccountId);
}