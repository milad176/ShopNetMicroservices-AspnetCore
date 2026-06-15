using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace BuildingBlocks.OpenTelemetry;

public sealed class Telemetry
{
    public Telemetry(string name)
    {
        Tracing = new ActivitySource(name);
        Metrics = new Meter(name);
    }

    /// <summary>
    /// https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/getting-started-aspnetcore
    /// </summary>
    public ActivitySource Tracing { get; }

    /// <summary>
    /// https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/metrics/getting-started-aspnetcore
    /// </summary>
    public Meter Metrics { get; }

    public void Dispose()
    {
        Metrics.Dispose();
        Tracing.Dispose();
    }
}