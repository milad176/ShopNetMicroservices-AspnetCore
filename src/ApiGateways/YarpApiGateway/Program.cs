using BuildingBlocks.HealthChecks;
using Common.Logging;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using YarpApiGateway.Common;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSeriLogging();

// Needed for correlation propagation
builder.Services.AddHttpContextAccessor();

// Add Reverse proxy (YARP) services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Services.AddHealthChecks(builder.Configuration);

var app = builder.Build();

// Correlation FIRST
app.UseMiddleware<CorrelationIdMiddleware>();

// Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseRateLimiter();
app.MapReverseProxy();
app.MapDefaultHealthChecks();

app.Run();