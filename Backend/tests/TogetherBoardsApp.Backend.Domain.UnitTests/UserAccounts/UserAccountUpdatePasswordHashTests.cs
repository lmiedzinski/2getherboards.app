using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests.UserAccounts;

public class UserAccountUpdatePasswordHashTests
{
    [Fact]
    public void UpdatePasswordHash_Throws_DeletedUserAccountUpdatesNotAllowedException_When_UserAccount_Is_Deleted()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.Delete();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.UpdatePasswordHash(string.Empty);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<DeletedUserAccountUpdatesNotAllowedException>();
        ((DeletedUserAccountUpdatesNotAllowedException)exception!).UserAccountId.Should().Be(userAccount.Id);
    }
    
    [Fact]
    public void UpdatePasswordHash_Updates_PasswordHash_On_Success()
    {
        // Arrange
        const string newPasswordHash = "passwordHash";
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.UpdatePasswordHash(newPasswordHash);
        });

        // Assert
        exception.Should().BeNull();
        userAccount.PasswordHash.Should().NotBeNull();
        userAccount.PasswordHash.Value.Should().Be(newPasswordHash);
    }
}