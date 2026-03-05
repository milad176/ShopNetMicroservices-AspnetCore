using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extentions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration,
        Assembly? assembly = null)
    {
        // Implement RabbitMQ MassTransit Configuration.
        return services;
    }
}