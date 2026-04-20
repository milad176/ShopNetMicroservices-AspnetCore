using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Common.Logging;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
                            ?? Guid.NewGuid().ToString();

        // Attach to HttpContext
        context.TraceIdentifier = correlationId;

        // Add to response header (important for debugging)
        context.Response.Headers[HeaderName] = correlationId;

        // Push into Serilog context
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            Activity.Current?.SetIdFormat(ActivityIdFormat.W3C);
            Activity.Current?.SetParentId(correlationId);

            await _next(context);
        }
    }
}