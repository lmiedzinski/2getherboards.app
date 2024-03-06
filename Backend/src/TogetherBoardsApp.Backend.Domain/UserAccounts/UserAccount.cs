using TogetherBoardsApp.Backend.Domain.Abstractions;
using TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;

namespace TogetherBoardsApp.Backend.Domain.UserAccounts;

public sealed class UserAccount : Entity
{
    private UserAccount()
    {
    }

    public UserAccountId Id { get; private set; } = null!;
    public UserAccountEmail Email { get; private set; } = null!;
    public UserAccountName Name { get; private set; } = null!;
    public UserAccountPasswordHash PasswordHash { get; private set; } = null!;
    public UserAccountRefreshToken? RefreshToken { get; private set; }

    public static UserAccount Create(
        UserAccountEmail email,
        UserAccountName name,
        UserAccountPasswordHash passwordHash)
    {
        var userAccount = new UserAccount
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = name,
            PasswordHash = passwordHash
        };

        userAccount.Raise(new UserAccountCreatedDomainEvent(Guid.NewGuid(), userAccount.Id));

        return userAccount;
    }
}