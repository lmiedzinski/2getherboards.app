namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public record UserAccountPasswordHash(string Value)
{
    public static implicit operator string(UserAccountPasswordHash userAccountPasswordHash) => userAccountPasswordHash.Value;
    public static implicit operator UserAccountPasswordHash(string userAccountPasswordHash) => new(userAccountPasswordHash);
}