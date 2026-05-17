using System.Text.Json;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildingBlocks.HealthChecks;

public static class HealthChecksExtension
{
    public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services)
    {
        var hcBuilder = services.AddHealthChecks();

        // Health check for the application itself
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);
        return hcBuilder;
    }

    public static void MapDefaultHealthChecks(this IEndpointRouteBuilder routes)
    {
        routes.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    service = AppDomain.CurrentDomain.FriendlyName,
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    status = report.Status.ToString(),
                    totalDuration = report.TotalDuration,
                    timestamp = DateTime.UtcNow,
                    entries = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        duration = e.Value.Duration,
                        description = e.Value.Description,
                        tags = e.Value.Tags
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));
            }
        });

        routes.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
    }
}