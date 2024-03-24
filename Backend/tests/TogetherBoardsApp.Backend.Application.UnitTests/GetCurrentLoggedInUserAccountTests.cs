using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TogetherBoardsApp.Backend.Application.Abstractions;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Application.Abstractions.UserAccounts;
using TogetherBoardsApp.Backend.Application.Exceptions;
using TogetherBoardsApp.Backend.Application.UserAccounts.GetCurrentLoggedInUserAccount;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using Xunit;

namespace TogetherBoardsApp.Backend.Application.UnitTests;

public class GetCurrentLoggedInUserAccountTests
{
    #region TestsSetup

    private readonly IUserAccountReadService _userAccountReadService;
    private readonly ITokenService _tokenService;
    private readonly IQueryHandler<
        GetCurrentLoggedInUserAccountQuery,
        GetCurrentLoggedInUserAccountQueryResponse> _queryHandler;

    public GetCurrentLoggedInUserAccountTests()
    {
        _userAccountReadService = Substitute.For<IUserAccountReadService>();
        _tokenService = Substitute.For<ITokenService>();

        _queryHandler = new GetCurrentLoggedInUserAccountQueryHandler(
            _userAccountReadService,
            _tokenService);
    }

    #endregion
    
    [Fact]
    public async Task Handle_Throws_NotFoundException_When_UserAccount_Not_Exists()
    {
        // Arrange
        var query = new GetCurrentLoggedInUserAccountQuery();
        var userId = Guid.NewGuid();
        _tokenService.GetUserAccountIdFromContext().Returns(new UserAccountId(userId));
        _userAccountReadService.GetUserAccountByIdAsync(new UserAccountId(userId)).ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _queryHandler.Handle(query, CancellationToken.None);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<NotFoundException>();
        exception!.Message.Should().Contain(userId.ToString());
    }
}