using BuildingBlocks.HealthChecks;

namespace Discount.Grpc.Common;

public static class HealthChecksExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDefaultHealthChecks();

        services.AddHealthChecks()
            .AddSqlite(connectionString: configuration.GetConnectionString("Database")!);

        return services;
    }
}