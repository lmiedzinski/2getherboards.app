using Microsoft.EntityFrameworkCore;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;

namespace TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework.Repositories;

internal sealed class UserAccountWriteRepository : IUserAccountWriteRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserAccountWriteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(
        UserAccount userAccount)
    {
        _dbContext.UserAccounts.Add(userAccount);
    }

    public async Task<UserAccount?> GetByIdAsync(
        UserAccountId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserAccounts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<UserAccount?> GetByEmailAsync(
        UserAccountEmail email,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserAccounts
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken: cancellationToken);
    }
}