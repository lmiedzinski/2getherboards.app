using TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts.DTO;
using TogetherBoardsApp.Backend.Domain.UserAccounts;

namespace TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts;

public interface IUserAccountReadService
{
    Task<UserAccountReadModelDto?> GetUserAccountByIdAsync(UserAccountId id);
}