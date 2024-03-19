using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.LogOutUserAccount;

public sealed class LogOutUserAccountCommandHandler : ICommandHandler<LogOutUserAccountCommand>
{
    private readonly ITokenService _tokenService;
    private readonly IUserAccountWriteRepository _userAccountWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogOutUserAccountCommandHandler(
        ITokenService tokenService,
        IUserAccountWriteRepository userAccountWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _userAccountWriteRepository = userAccountWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogOutUserAccountCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _tokenService.GetUserAccountIdFromContext();

        var userAccount = await _userAccountWriteRepository.GetByIdAsync(userAccountId, cancellationToken);
        if (userAccount is null)
            throw new NotFoundException(nameof(UserAccount), userAccountId.Value.ToString());
        
        userAccount.RevokeRefreshToken();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}