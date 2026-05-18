using BuildingBlocks.HealthChecks;

namespace Ordering.API.Common;

public static class HealthChecksExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultHealthChecks();

        var rabbitConfig = configuration.GetSection("MessageBroker");

        var rabbitUri = rabbitConfig["Host"];
        var rabbitUser = rabbitConfig["UserName"];
        var rabbitPass = rabbitConfig["Password"];

        var rabbitConnection =
            new UriBuilder(rabbitUri!)

            {
                UserName = rabbitUser,

                Password = rabbitPass
            }.Uri.ToString();

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database")!, name: "sqlserver", tags: ["ready"])
            .AddRabbitMQ(rabbitConnection, name: "rabbitmq", tags: ["ready"]);

        return services;
    }
}