using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;

public sealed class LogInUserAccountCommandHandler : ICommandHandler<LogInUserAccountCommand, LogInUserAccountResponse>
{
    private readonly IUserAccountWriteRepository _userAccountWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyService _cryptographyService;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LogInUserAccountCommandHandler(
        IUserAccountWriteRepository userAccountWriteRepository,
        IUnitOfWork unitOfWork,
        ICryptographyService cryptographyService,
        ITokenService tokenService,
        IDateTimeProvider dateTimeProvider)
    {
        _userAccountWriteRepository = userAccountWriteRepository;
        _unitOfWork = unitOfWork;
        _cryptographyService = cryptographyService;
        _tokenService = tokenService;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<LogInUserAccountResponse> Handle(LogInUserAccountCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _userAccountWriteRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (userAccount is null)
            throw new EmailOrPasswordIncorrectException(request.Email);

        if (!_cryptographyService.IsPasswordMatchingHash(request.Password, userAccount.PasswordHash.Value))
            throw new EmailOrPasswordIncorrectException(request.Email);
        
        userAccount.SetNewRefreshToken(
            _tokenService.GenerateRefreshToken(),
            _dateTimeProvider.UtcNow.AddMinutes(_tokenService.GetRefreshTokenLifetimeInMinutes()));

        var accessToken = _tokenService.GenerateAccessToken(userAccount.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LogInUserAccountResponse(accessToken, userAccount.RefreshToken!.Value);
    }
}