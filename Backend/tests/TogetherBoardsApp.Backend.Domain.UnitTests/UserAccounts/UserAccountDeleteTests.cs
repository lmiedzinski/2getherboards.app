using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests.UserAccounts;

public class UserAccountDeleteTests
{
    [Fact]
    public void Delete_DoesNothing_When_IsDeleted_True_On_Success()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.Delete();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.Delete();
        });

        // Assert
        exception.Should().BeNull();
        userAccount.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public void Delete_Sets_IsDeleted_When_IsDeleted_False_On_Success()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.Delete();
        });

        // Assert
        exception.Should().BeNull();
        userAccount.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public void Delete_Sets_RefreshToken_IsActive_False_When_RefreshToken_Is_Not_Null_On_Success()
    {
        // Arrange
        const string refreshTokenValue = "refreshTokenValue";
        var refreshTokenExpirationDate = DateTime.UtcNow;
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.SetNewRefreshToken(refreshTokenValue, refreshTokenExpirationDate);

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.Delete();
        });

        // Assert
        exception.Should().BeNull();
        userAccount.RefreshToken.Should().NotBeNull();
        userAccount.RefreshToken!.IsActive.Should().BeFalse();
        userAccount.RefreshToken!.Value.Should().Be(refreshTokenValue);
        userAccount.RefreshToken!.ExpirationDateUtc.Should().Be(refreshTokenExpirationDate);
    }
    
    [Fact]
    public void Delete_Adds_UserAccountDeletedDomainEvent_On_Success()
    {
        // Arrange
        var userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        userAccount.ClearDomainEvents();

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount.Delete();
        });

        // Assert
        exception.Should().BeNull();
        var domainEvents = userAccount.GetDomainEvents();
        domainEvents.Should().NotBeNull().And.HaveCount(1);
        var domainEvent = domainEvents.First();
        domainEvent.Should().BeOfType<UserAccountDeletedDomainEvent>();
        domainEvent.Id.Should().NotBe(Guid.Empty);
        ((UserAccountDeletedDomainEvent)domainEvent).UserAccountId.Should().Be(userAccount.Id);
    }
}