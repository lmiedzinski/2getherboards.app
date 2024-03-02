using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TogetherBoardsApp.Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssembly>();

            // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            // config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly);

        return services;
    }
}