using System.Reflection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class SeriLogger
{
    public static IHostBuilder UseSeriLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            var env = context.HostingEnvironment;
            var appName = Assembly.GetEntryAssembly()?.GetName().Name;

            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                    new Uri(context.Configuration["ElasticConfiguration:Uri"]!))
                {
                    AutoRegisterTemplate = true,
                    NumberOfReplicas = 1,
                    NumberOfShards = 2,
                    IndexFormat =
                        $"applogs-{appName?.ToLower().Replace(".", "-")}-{env.EnvironmentName?.ToLower()}-logs-{DateTime.UtcNow:yyyy-MM}"
                });
        });

        return hostBuilder;
    }
}