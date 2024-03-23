using Dapper;
using TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts;
using TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts.DTO;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Infrastructure.Database.SqlConnection;

namespace TogetherBoardsApp.Backend.Infrastructure.Database.Dapper.Repositories;

internal sealed class UserAccountReadService : IUserAccountReadService
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public UserAccountReadService(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<UserAccountReadModelDto?> GetUserAccountByIdAsync(UserAccountId id)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                             ua.id AS Id,
                             ua.email AS Email,
                             ua.name AS Name
                           FROM user_accounts ua
                           WHERE ua.id = @UserAccountId
                           """;

        var userAccount = await connection.QueryFirstOrDefaultAsync<UserAccountReadModelDto>(
            sql,
            new
            {
                UserAccountId = id.Value
            });

        return userAccount;
    }
}