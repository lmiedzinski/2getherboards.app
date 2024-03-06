namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public sealed record UserAccountName(string Value)
{
    public static implicit operator string(UserAccountName userAccountName) => userAccountName.Value;
    public static implicit operator UserAccountName(string userAccountName) => new(userAccountName);
}