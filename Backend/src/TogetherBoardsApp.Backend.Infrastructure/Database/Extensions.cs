using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TogetherBoardsApp.Backend.Domain.UnitOfWork;
using TogetherBoardsApp.Backend.Domain.UserAccounts.Repositories;
using TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework;
using TogetherBoardsApp.Backend.Infrastructure.Database.EntityFramework.Repositories;
using TogetherBoardsApp.Backend.Infrastructure.Database.SqlConnection;

namespace TogetherBoardsApp.Backend.Infrastructure.Database;

internal static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("TogetherBoardsAppDb") ??
            throw new ArgumentNullException(nameof(configuration));
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging();
        });
        
        services.AddScoped<IUserAccountWriteRepository, UserAccountWriteRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        
        services.AddHostedService<DatabaseInitializer>();
        
        services.AddSingleton(_ => new SqlConnectionFactory(connectionString));
        
        return services;
    }
}