using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;
using TogetherBoardsApp.Backend.śApplication.UserAccounts.RefreshUserAccountToken;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.RefreshUserAccountToken;

public sealed class RefreshUserAccountTokenCommandHandler
    : ICommandHandler<RefreshUserAccountTokenCommand, RefreshUserAccountTokenResponse>
{
    private readonly IUserAccountWriteRepository _userAccountWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshUserAccountTokenCommandHandler(
        IUserAccountWriteRepository userAccountWriteRepository,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IDateTimeProvider dateTimeProvider)
    {
        _userAccountWriteRepository = userAccountWriteRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RefreshUserAccountTokenResponse> Handle(
        RefreshUserAccountTokenCommand request,
        CancellationToken cancellationToken)
    {
        var userAccount = await _userAccountWriteRepository.GetByActiveRefreshTokenValueAsync(
            request.RefreshToken,
            cancellationToken);
        if (userAccount is null) throw new NotFoundException(nameof(UserAccountRefreshToken));
        
        userAccount.SetNewRefreshToken(
            userAccount.RefreshToken!.Value,
            _dateTimeProvider.UtcNow.AddMinutes(_tokenService.GetRefreshTokenLifetimeInMinutes()));

        var accessToken = _tokenService.GenerateAccessToken(userAccount.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshUserAccountTokenResponse(accessToken);
    }
}