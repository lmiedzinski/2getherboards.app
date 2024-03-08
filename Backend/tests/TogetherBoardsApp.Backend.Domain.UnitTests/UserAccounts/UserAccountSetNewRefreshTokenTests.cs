using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests.UserAccounts;

public class UserAccountSetNewRefreshTokenTests
{
    [Fact]
    public void SetNewRefreshToken_Throws_DeletedUserAccountUpdatesNotAllowedException_When_UserAccount_Is_Deleted()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.Delete();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.SetNewRefreshToken(string.Empty, DateTime.UtcNow);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<DeletedUserAccountUpdatesNotAllowedException>();
        ((DeletedUserAccountUpdatesNotAllowedException)exception!).UserAccountId.Should().Be(userAccount.Id);
    }
    
    [Fact]
    public void SetNewRefreshToken_Sets_RefreshToken_On_Success()
    {
        // Arrange
        const string refreshTokenValue = "refreshTokenValue";
        var refreshTokenExpirationDate = DateTime.UtcNow;
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.SetNewRefreshToken(refreshTokenValue, refreshTokenExpirationDate);
        });

        // Assert
        exception.Should().BeNull();
        userAccount.RefreshToken.Should().NotBeNull();
        userAccount.RefreshToken!.IsActive.Should().BeTrue();
        userAccount.RefreshToken!.Value.Should().Be(refreshTokenValue);
        userAccount.RefreshToken!.ExpirationDateUtc.Should().Be(refreshTokenExpirationDate);
    }
}