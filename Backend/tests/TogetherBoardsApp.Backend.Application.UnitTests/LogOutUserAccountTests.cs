using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Application.UserAccounts.LogOutUserAccount;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;
using Xunit;

namespace TogetherBoardsApp.Backend.Application.UnitTests;

public class LogOutUserAccountTests
{
    #region TestsSetup

    private readonly IUserAccountWriteRepository _userAccountWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICommandHandler<LogOutUserAccountCommand> _commandHandler;

    public LogOutUserAccountTests()
    {
        _userAccountWriteRepository = Substitute.For<IUserAccountWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _tokenService = Substitute.For<ITokenService>();

        _commandHandler = new LogOutUserAccountCommandHandler(
            _tokenService,
            _userAccountWriteRepository,
            _unitOfWork);
    }

    #endregion
    
    [Fact]
    public async Task Handle_Throws_NotFoundException_When_UserAccount_Is_Null()
    {
        // Arrange
        var command = new LogOutUserAccountCommand();
        var userAccountId = new UserAccountId(Guid.NewGuid());
        _tokenService.GetUserAccountIdFromContext().Returns(userAccountId);
        _userAccountWriteRepository.GetByIdAsync(userAccountId).ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _commandHandler.Handle(command, CancellationToken.None);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<NotFoundException>();
        exception!.Message.Should().Contain(nameof(UserAccount)).And.Contain(userAccountId.Value.ToString());
    }
    
    [Fact]
    public async Task Handle_Calls_Once_UnitOfWork_On_Success()
    {
        // Arrange
        var command = new LogOutUserAccountCommand();
        var userAccountId = new UserAccountId(Guid.NewGuid());
        _tokenService.GetUserAccountIdFromContext().Returns(userAccountId);
        _userAccountWriteRepository.GetByIdAsync(userAccountId)
            .Returns(UserAccount.Create("testuser@email.com", "Test User", "testPassword123"));
        
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