using System.Reflection;
using BuildingBlocks.CQRS.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.RegisterMediateR(Assembly.GetExecutingAssembly());

        return services;
    }
}