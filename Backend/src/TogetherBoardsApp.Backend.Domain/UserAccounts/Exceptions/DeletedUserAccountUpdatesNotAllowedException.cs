using TogetherBoardsApp.Backend.Domain.Abstractions;

namespace TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;

public sealed class DeletedUserAccountUpdatesNotAllowedException : DomainException
{
    public UserAccountId UserAccountId { get; private set; }
    
    public DeletedUserAccountUpdatesNotAllowedException(UserAccountId id)
        : base($"User account with the id {id.Value} is deleted and can't be modified")
    {
        UserAccountId = id;
    }
}