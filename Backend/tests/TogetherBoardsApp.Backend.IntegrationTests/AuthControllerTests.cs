using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TogetherBoardsApp.Backend.Api.UserAccounts.Requests;
using TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;
using TogetherBoardsApp.Backend.IntegrationTests.TestsSetup;
using Xunit;

namespace TogetherBoardsApp.Backend.IntegrationTests;

public class AuthControllerTests : BaseTest
{
    public AuthControllerTests(TogetherBoardsAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task LogIn_Returns_LogInUserAccountResponse_On_Success()
    {
        // Arrange
        const string email = "testuser@email.com";
        const string name = "Test User";
        const string password = "testPassword123";
        
        await CreateTestUserAsync(email, name, password);
        
        var request = new LogInRequest(email, password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/auth/log-in", request);
        var logInUserAccountResponse = await response.Content.ReadFromJsonAsync<LogInUserAccountResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        logInUserAccountResponse.Should().NotBeNull();
        logInUserAccountResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
        logInUserAccountResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }
}