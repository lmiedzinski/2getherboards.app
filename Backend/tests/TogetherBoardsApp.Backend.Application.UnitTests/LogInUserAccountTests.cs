using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;
using Xunit;

namespace TogetherBoardsApp.Backend.Application.UnitTests;

public class LogInUserAccountTests
{
    #region TestsSetup

    private readonly IUserAccountWriteRepository _userAccountWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyService _cryptographyService;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICommandHandler<LogInUserAccountCommand, LogInUserAccountResponse> _commandHandler;

    public LogInUserAccountTests()
    {
        _userAccountWriteRepository = Substitute.For<IUserAccountWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cryptographyService = Substitute.For<ICryptographyService>();
        _tokenService = Substitute.For<ITokenService>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        _commandHandler = new LogInUserAccountCommandHandler(
            _userAccountWriteRepository,
            _unitOfWork,
            _cryptographyService,
            _tokenService,
            _dateTimeProvider);
    }

    #endregion
    
    [Fact]
    public async Task Handle_Throws_EmailOrPasswordIncorrectException_When_UserAccount_Is_Null()
    {
        // Arrange
        var command = new LogInUserAccountCommand("test@email.com", "password");
        _userAccountWriteRepository.GetByEmailAsync(command.Email).ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _commandHandler.Handle(command, CancellationToken.None);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<EmailOrPasswordIncorrectException>();
        exception!.Message.Should().Contain(command.Email);
    }
    
    [Fact]
    public async Task Handle_Throws_EmailOrPasswordIncorrectException_When_Password_Is_Incorrect()
    {
        // Arrange
        const string passwordHash = "passwordHash";
        var command = new LogInUserAccountCommand("test@email.com", "password");
        _userAccountWriteRepository.GetByEmailAsync(command.Email)
            .Returns(UserAccount.Create(command.Email, "name", passwordHash));
        _cryptographyService.IsPasswordMatchingHash(command.Password, passwordHash).Returns(false);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _commandHandler.Handle(command, CancellationToken.None);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<EmailOrPasswordIncorrectException>();
        exception!.Message.Should().Contain(command.Email);
    }
    
    [Fact]
    public async Task Handle_Returns_LogInUserAccountResponse_On_Success()
    {
        // Arrange
        const string passwordHash = "passwordHash";
        const string refreshToken = "refreshToken";
        const string accessToken = "accessToken";
        const int refreshTokenLifespanInMinutes = 128;
        var dateTimeNow = DateTime.UtcNow;
        var command = new LogInUserAccountCommand("test@email.com", "password");
        var userAccount = UserAccount.Create(command.Email, "name", passwordHash);
        _userAccountWriteRepository.GetByEmailAsync(command.Email)
            .Returns(userAccount);
        _cryptographyService.IsPasswordMatchingHash(command.Password, passwordHash).Returns(true);
        _tokenService.GenerateRefreshToken().Returns(refreshToken);
        _tokenService.GetRefreshTokenLifetimeInMinutes().Returns(refreshTokenLifespanInMinutes);
        _tokenService.GenerateAccessToken(userAccount.Id).Returns(accessToken);
        _dateTimeProvider.UtcNow.Returns(dateTimeNow);
        
        // Act
        LogInUserAccountResponse? result = null;
        var exception = await Record.ExceptionAsync(async () =>
        {
            result = await _commandHandler.Handle(command, CancellationToken.None);
        });

        // Assert
        exception.Should().BeNull();
        result.Should().NotBeNull();
        result!.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);
    }
    
    [Fact]
    public async Task Handle_Calls_Once_UnitOfWork_On_Success()
    {
        // Arrange
        const string passwordHash = "passwordHash";
        const string refreshToken = "refreshToken";
        const string accessToken = "accessToken";
        const int refreshTokenLifespanInMinutes = 128;
        var dateTimeNow = DateTime.UtcNow;
        var command = new LogInUserAccountCommand("test@email.com", "password");
        var userAccount = UserAccount.Create(command.Email, "name", passwordHash);
        _userAccountWriteRepository.GetByEmailAsync(command.Email)
            .Returns(userAccount);
        _cryptographyService.IsPasswordMatchingHash(command.Password, passwordHash).Returns(true);
        _tokenService.GenerateRefreshToken().Returns(refreshToken);
        _tokenService.GetRefreshTokenLifetimeInMinutes().Returns(refreshTokenLifespanInMinutes);
        _tokenService.GenerateAccessToken(userAccount.Id).Returns(accessToken);
        _dateTimeProvider.UtcNow.Returns(dateTimeNow);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _commandHandler.Handle(command, CancellationToken.None);
        });

        // Assert
        exception.Should().BeNull();
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}