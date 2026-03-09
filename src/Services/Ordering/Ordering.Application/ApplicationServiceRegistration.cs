using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterMediateR(Assembly.GetExecutingAssembly());
        services.AddFeatureManagement();
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}