using TogetherBoardsApp.Backend.Domain.Abstractions;
using TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;

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
    public bool IsDeleted { get; private set; }

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
    
    public void SetNewRefreshToken(string value, DateTime expiryDate)
    {
        CheckIfUserAccountUpdatesAreAllowed();
        
        RefreshToken = new UserAccountRefreshToken(value, true, expiryDate);
    }

    public void RevokeRefreshToken()
    {
        CheckIfUserAccountUpdatesAreAllowed();
        
        if (RefreshToken is null) return;
        RefreshToken = RefreshToken with { IsActive = false };
    }
    
    public void UpdatePasswordHash(UserAccountPasswordHash passwordHash)
    {
        CheckIfUserAccountUpdatesAreAllowed();
        
        PasswordHash = passwordHash;
    }
    
    public void UpdateName(UserAccountName name)
    {
        CheckIfUserAccountUpdatesAreAllowed();
        
        Name = name;
    }
    
    public void Delete()
    {
        if (IsDeleted) return;

        IsDeleted = true;
        if (RefreshToken is { IsActive: true }) RefreshToken = RefreshToken with { IsActive = false };

        Raise(new UserAccountDeletedDomainEvent(Guid.NewGuid(), Id));
    }
    
    private void CheckIfUserAccountUpdatesAreAllowed()
    {
        if(IsDeleted) throw new DeletedUserAccountUpdatesNotAllowedException(Id);
    }
}