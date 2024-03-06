using FluentAssertions;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;
using Xunit;

namespace TogetherBoardsApp.Backend.Domain.UnitTests;

public class UserAccountCreateTests
{
    [Fact]
    public void Create_Returns_UserAccount_Object_With_Correct_Properties_On_Success()
    {
        // Arrange
        const string email = "userlogin@domain.com";
        const string name = "Test User";
        const string passwordHash = "SuperSecretPasswordHash";
        UserAccount? userAccount = null;

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount = UserAccount.Create(email, name, passwordHash);
        });

        // Assert
        exception.Should().BeNull();
        userAccount.Should().NotBeNull();
        userAccount!.Email.Should().NotBeNull();
        userAccount!.Email.Value.Should().Be(email);
        userAccount!.Name.Should().NotBeNull();
        userAccount!.Name.Value.Should().Be(name);
        userAccount!.PasswordHash.Should().NotBeNull();
        userAccount!.PasswordHash.Value.Should().Be(passwordHash);
        userAccount!.Id.Should().NotBeNull().And.NotBe(Guid.Empty);
        userAccount!.RefreshToken.Should().BeNull();
    }

    [Fact]
    public void Create_Adds_UserAccountCreatedDomainEvent_On_Success()
    {
        // Arrange
        UserAccount? userAccount = null;

        // Act
        var exception = Record.Exception(() =>
        {
            userAccount = UserAccount.Create(string.Empty, string.Empty, string.Empty);
        });

        // Assert
        exception.Should().BeNull();
        userAccount.Should().NotBeNull();
        var domainEvents = userAccount!.GetDomainEvents();
        domainEvents.Should().NotBeNull().And.HaveCount(1);
        var domainEvent = domainEvents.First();
        domainEvent.Should().BeOfType<UserAccountCreatedDomainEvent>();
        domainEvent.Id.Should().NotBe(Guid.Empty);
        ((UserAccountCreatedDomainEvent)domainEvent).UserAccountId.Should().Be(userAccount.Id);
    }
}