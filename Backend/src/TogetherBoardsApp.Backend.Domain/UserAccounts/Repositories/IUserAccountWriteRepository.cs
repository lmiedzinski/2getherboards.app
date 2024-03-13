namespace TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;

public interface IUserAccountWriteRepository
{
    void Add(UserAccount userAccount);
    
    Task<UserAccount?> GetByIdAsync(
        UserAccountId id,
        CancellationToken cancellationToken = default);
    
    Task<UserAccount?> GetByEmailAsync(
        UserAccountEmail email,
        CancellationToken cancellationToken = default);
}