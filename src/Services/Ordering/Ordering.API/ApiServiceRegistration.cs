using BuildingBlocks.Exceptions.Handler;
using Carter;

namespace Ordering.API;

public static class ApiServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseProblemDetailsResponseExceptionHandler();
        return app;
    }
}