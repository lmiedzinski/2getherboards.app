using Microsoft.Extensions.DependencyInjection;
using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Domain.UserAccounts;
using TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework;
using Xunit;

namespace TogetherBoardsApp.Backend.IntegrationTests.TestsSetup;

[Collection("SharedTestCollection")]
public abstract class BaseTest : IAsyncLifetime
{
    private readonly ICryptographyService _cryptographyService;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient HttpClient;
    protected readonly ApplicationDbContext DbContext;
    
    protected BaseTest(TogetherBoardsAppFactory webAppFactory)
    {
        HttpClient = webAppFactory.HttpClient;
        _resetDatabase = webAppFactory.ResetDatabaseAsync;
        
        var serviceScope = webAppFactory.Services.CreateScope();
        DbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _cryptographyService = serviceScope.ServiceProvider.GetRequiredService<ICryptographyService>();
        _tokenService = serviceScope.ServiceProvider.GetRequiredService<ITokenService>();
        _dateTimeProvider = serviceScope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
    }

    protected async Task<UserAccount> CreateTestUserAsync(
        string email = "testuser@email.com",
        string name = "Test User",
        string password = "Test123456789",
        Dictionary<string, string>? claims = null)
    {
        var testUser = UserAccount.Create(
            new UserAccountEmail(email),
            new UserAccountName(name),
            new UserAccountPasswordHash(_cryptographyService.HashPassword(password)));
        
        testUser.SetNewRefreshToken(
            _tokenService.GenerateRefreshToken(),
            _dateTimeProvider.UtcNow.AddMinutes(_tokenService.GetRefreshTokenLifetimeInMinutes()));
        
        testUser.ClearDomainEvents();
        
        await DbContext.UserAccounts.AddAsync(testUser);
        await DbContext.SaveChangesAsync();
        
        return testUser;
    }
    
    protected string GenerateAccessTokenForUser(Guid userAccountId)
    {
        return _tokenService.GenerateAccessToken(new UserAccountId(userAccountId));
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}