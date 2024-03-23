using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using TogetherBoardsApp.Backend.Application.UserAccounts.GetCurrentLoggedInUserAccount;
using TogetherBoardsApp.Backend.IntegrationTests.TestsSetup;
using Xunit;

namespace TogetherBoardsApp.Backend.IntegrationTests;

public class UserAccountsControllerTests : BaseTest
{
    public UserAccountsControllerTests(TogetherBoardsAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task GetCurrentLoggedInUserAccount_Returns_GetCurrentLoggedInUserAccountQueryResponse_On_Success()
    {
        // Arrange
        const string email = "testuser@email.com";
        const string name = "Test User";
        const string password = "testPassword123";

        var testUser = await CreateTestUserAsync(email, name, password);
        var accessToken = GenerateAccessTokenForUser(testUser.Id.Value);

        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.GetAsync("api/user-accounts/me");
        var getCurrentLoggedInUserAccountQueryResponse =
            await response.Content.ReadFromJsonAsync<GetCurrentLoggedInUserAccountQueryResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getCurrentLoggedInUserAccountQueryResponse.Should().NotBeNull();
        getCurrentLoggedInUserAccountQueryResponse!.Id.Should().Be(testUser.Id.Value);
        getCurrentLoggedInUserAccountQueryResponse.Email.Should().Be(testUser.Email.Value);
        getCurrentLoggedInUserAccountQueryResponse.Name.Should().Be(testUser.Name.Value);
    }
}