using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests.UserAccounts;

public class UserAccountRevokeRefreshTokenTests
{
    [Fact]
    public void RevokeRefreshToken_Throws_DeletedUserAccountUpdatesNotAllowedException_When_UserAccount_Is_Deleted()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.Delete();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.RevokeRefreshToken();
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<DeletedUserAccountUpdatesNotAllowedException>();
        ((DeletedUserAccountUpdatesNotAllowedException)exception!).UserAccountId.Should().Be(userAccount.Id);
    }
    
    [Fact]
    public void RevokeRefreshToken_DoesNothing_When_RefreshToken_Is_Null_On_Success()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.RevokeRefreshToken();
        });

        // Assert
        exception.Should().BeNull();
        userAccount.RefreshToken.Should().BeNull();
    }
    
    [Fact]
    public void RevokeRefreshToken_Sets_IsActive_False_When_RefreshToken_Is_Not_Null_On_Success()
    {
        // Arrange
        const string refreshTokenValue = "refreshTokenValue";
        var refreshTokenExpirationDate = DateTime.UtcNow;
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.SetNewRefreshToken(refreshTokenValue, refreshTokenExpirationDate);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.RevokeRefreshToken();
        });

        // Assert
        exception.Should().BeNull();
        userAccount.RefreshToken.Should().NotBeNull();
        userAccount.RefreshToken!.IsActive.Should().BeFalse();
        userAccount.RefreshToken!.Value.Should().Be(refreshTokenValue);
        userAccount.RefreshToken!.ExpirationDateUtc.Should().Be(refreshTokenExpirationDate);
    }
}