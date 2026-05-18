using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.HealthChecks;
using Ordering.API.Common;

namespace Ordering.API;

public static class ApiServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddHealthChecks(configuration);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseProblemDetailsResponseExceptionHandler();
        app.MapDefaultHealthChecks();

        return app;
    }
}