using BuildingBlocks.HealthChecks;

namespace Catalog.API.Common;

public static class HealthChecksExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultHealthChecks();

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!, name: "postgres", tags: ["ready", "liveness"]);

        return services;
    }
}
