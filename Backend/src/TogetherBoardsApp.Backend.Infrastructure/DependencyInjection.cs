using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using TogetherBoardsApp.Backend.Application.Abstractions.Cryptography;
using TogetherBoardsApp.Backend.Application.Abstractions.DateAndTime;
using TogetherBoardsApp.Backend.Application.Abstractions.Token;
using TogetherBoardsApp.Backend.Infrastructure.Cryptography;
using TogetherBoardsApp.Backend.Infrastructure.DateAndTime;
using TogetherBoardsApp.Backend.Infrastructure.Middlewares;
using TogetherBoardsApp.Backend.Infrastructure.Swagger;
using TogetherBoardsApp.Backend.Infrastructure.Token;

namespace TogetherBoardsApp.Backend.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));
        
        services.AddQuartz();
        
        services.AddEndpointsApiExplorer();
        services.AddConfiguredSwagger();
        
        services.AddHttpContextAccessor();
        services.AddHealthChecks();
        
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<RsaKeysProvider>();
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        services.AddAuthorization();
        
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