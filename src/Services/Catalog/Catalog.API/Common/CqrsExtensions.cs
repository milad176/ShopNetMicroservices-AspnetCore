using System.Reflection;

namespace Catalog.API.Common;

public static class CqrsExtensions
{
    public static void RegisterMediateR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });
    }
}
