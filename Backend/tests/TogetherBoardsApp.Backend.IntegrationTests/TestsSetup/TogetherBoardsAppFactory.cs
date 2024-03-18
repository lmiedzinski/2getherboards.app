using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework;
using TogetherBoardsApp.Backend.Infrastructure.Database.SqlConnection;
using Xunit;

namespace TogetherBoardsApp.Backend.IntegrationTests.TestsSetup;

public sealed class TogetherBoardsAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresDatabase = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("together_boards_app")
        .WithUsername("sa")
        .WithPassword("Test123456789")
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_postgresDatabase.GetConnectionString());
            });

            services.RemoveAll(typeof(SqlConnectionFactory));
            services.AddSingleton(_ => new SqlConnectionFactory(_postgresDatabase.GetConnectionString()));
        });
            
        builder.UseEnvironment("Test");
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _postgresDatabase.StartAsync();
        _dbConnection = new NpgsqlConnection(_postgresDatabase.GetConnectionString());
        
        HttpClient = CreateClient();

        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public new async Task DisposeAsync()
    {
        await _postgresDatabase.StopAsync();
    }
}