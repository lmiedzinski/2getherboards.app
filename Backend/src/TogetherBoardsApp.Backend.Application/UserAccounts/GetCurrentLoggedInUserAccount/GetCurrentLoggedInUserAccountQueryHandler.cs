using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.GetCurrentLoggedInUserAccount;

public sealed class GetCurrentLoggedInUserAccountQueryHandler
    : IQueryHandler<GetCurrentLoggedInUserAccountQuery, GetCurrentLoggedInUserAccountQueryResponse>
{
    private readonly IUserAccountReadService _userAccountReadService;
    private readonly ITokenService _tokenService;

    public GetCurrentLoggedInUserAccountQueryHandler(
        IUserAccountReadService userAccountReadService,
        ITokenService tokenService)
    {
        _userAccountReadService = userAccountReadService;
        _tokenService = tokenService;
    }

    public async Task<GetCurrentLoggedInUserAccountQueryResponse> Handle(
        GetCurrentLoggedInUserAccountQuery request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _tokenService.GetUserAccountIdFromContext();
        var userAccount = await _userAccountReadService.GetUserAccountByIdAsync(userAccountId);
        if (userAccount is null) throw new NotFoundException(nameof(UserAccount), userAccountId.Value.ToString());

        return new GetCurrentLoggedInUserAccountQueryResponse(
            userAccount.Id,
            userAccount.Email,
            userAccount.Name);
    }
}