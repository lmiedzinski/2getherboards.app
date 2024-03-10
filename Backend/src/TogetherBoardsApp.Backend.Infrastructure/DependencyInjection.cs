using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Infrastructure.Cryptography;
using TogetherBoardsApp.Backend.Infrastructure.DateAndTime;
using TogetherBoardsApp.Backend.Infrastructure.Middlewares;
using TogetherBoardsApp.Backend.Infrastructure.Swagger;

namespace TogetherBoardsApp.Backend.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();
        
        services.AddEndpointsApiExplorer();
        services.AddConfiguredSwagger();
        
        services.AddHttpContextAccessor();
        services.AddHealthChecks();
        
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        
        services.AddControllers();
    }

    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseConfiguredSwagger();
        
        app.MapHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
    }
}