using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BuildingBlocks.OpenTelemetry;

public static class OpenTelemetryExtensions
{
    /// <summary>
        /// Adds Open Telemetry metrics, tracing and logging
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceName"></param>
        public static void AddOpenTelemetryOtl(this IServiceCollection service, string serviceName)
        {
            var telemetry = new Telemetry(serviceName);
            service.AddSingleton(_ => telemetry);
            
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);
            service.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
            {
                options.RecordException = true;
                // Filter out instrumentation of the Prometheus scraping endpoint.
                options.Filter = ctx => ctx.Request.Path != "/metrics";
            });

            service.AddOpenTelemetry()
                .ConfigureResource(b =>
                {
                    b.AddService(serviceName);
                })
                .WithTracing(b => b
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                    .AddHttpClientInstrumentation(options => options.RecordException = true)
                    .AddGrpcClientInstrumentation()
                    .AddMassTransitInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource(telemetry.Tracing.Name)
                    .AddOtlpExporter())
                .WithMetrics(b => b
                    .AddMeter(telemetry.Metrics.Name)
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddPrometheusExporter())
                .WithLogging(b => b
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter());
        }
}