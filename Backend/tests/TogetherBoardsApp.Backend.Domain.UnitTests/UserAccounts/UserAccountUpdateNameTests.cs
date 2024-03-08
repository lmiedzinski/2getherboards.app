using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Exceptions;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests.UserAccounts;

public class UserAccountUpdateNameTests
{
    [Fact]
    public void UpdateName_Throws_DeletedUserAccountUpdatesNotAllowedException_When_UserAccount_Is_Deleted()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.Delete();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.UpdateName(string.Empty);
        });

        // Assert
        exception.Should().NotBeNull().And.BeOfType<DeletedUserAccountUpdatesNotAllowedException>();
        ((DeletedUserAccountUpdatesNotAllowedException)exception!).UserAccountId.Should().Be(userAccount.Id);
    }
    
    [Fact]
    public void UpdateName_Updates_Name_On_Success()
    {
        // Arrange
        const string newName = "name";
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.UpdateName(newName);
        });

        // Assert
        exception.Should().BeNull();
        userAccount.Name.Should().NotBeNull();
        userAccount.Name.Value.Should().Be(newName);
    }
}