using BuildingBlocks.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shopping.Web.Common;

public static class HealthChecksExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDefaultHealthChecks();

        var hcServices = configuration.GetSection("HealthCheckServices");
        var builder = services.AddHealthChecks();

        foreach (var service in hcServices.GetChildren())
        {
            var name = service.Key;
            var address = service.Value;

            if (string.IsNullOrWhiteSpace(address))
                continue;

            builder.AddUrlGroup(new Uri($"{address}/hc"), name: name, HealthStatus.Degraded);
        }

        return services;
    }
}