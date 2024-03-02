using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TogetherBoardsApp.Backend.Infrastructure.Swagger;

internal static class Swagger
{
    private const string Name = "2getherBoards.app Backend API";
    
    public static void AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = Assembly.GetEntryAssembly()?.GetName().Name,
                    Description = Name,
                    Contact = new OpenApiContact
                    {
                        Name = "Łukasz Miedziński",
                        Email = string.Empty,
                        Url = new Uri("https://lukaszmiedzinski.com")
                    }
                });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme.<br>
                                        Please enter JWT into field (without Bearer)",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
            c.OperationFilter<SwaggerAuthorizationHeaderFilter>();
            c.CheckIfExistsAndIncludeXmlComments();
        });
    }

    public static void UseConfiguredSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", Name));
    }

    private static void CheckIfExistsAndIncludeXmlComments(this SwaggerGenOptions c)
    {
        var basePath = AppContext.BaseDirectory;
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
        var fileName = Path.GetFileName(assemblyName + ".xml");
        var path = Path.Combine(basePath, fileName);

        if (File.Exists(path))
        {
            c.IncludeXmlComments(path, true);
        }
    }
}