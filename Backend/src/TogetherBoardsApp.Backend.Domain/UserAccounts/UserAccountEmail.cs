namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public sealed record UserAccountEmail(string Value)
{
    public static implicit operator string(UserAccountEmail userAccountEmail) => userAccountEmail.Value;
    public static implicit operator UserAccountEmail(string userAccountEmail) => new(userAccountEmail);
}