using Basket.API.Common;
using Basket.API.Interceptors;
using Basket.API.Models.Configs;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.HealthChecks;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Resilience.gRPC;
using Common.Logging;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;
using Serilog;
using Shared;


var builder = WebApplication.CreateBuilder(args);

// Serilog from shared library
builder.Host.UseSeriLogging();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<GrpcCorrelationInterceptor>();
builder.Services.RegisterMediateR(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.AddDefaultOpenApi();
builder.Services.Configure<UrlsConfig>(builder.Configuration.GetSection("urls"));
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().UseNumericRevisions(true);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((service, option) =>
    {
        var discountUrl = service.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcDiscount;
        option.Address = new Uri(discountUrl);
    })
    .AddInterceptor<GrpcCorrelationInterceptor>()
    .AddPolicyHandler((sp, request) =>
    {
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("GrpcPolicy");
        return GrpcResiliencePolicies.CreateGrpcRetryPolicy(logger);
    });

//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddHealthChecks(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

app.UseRouting();
app.UseProblemDetailsResponseExceptionHandler();
app.MapDefaultHealthChecks();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDefaultOpenApi();
}

app.MapGroup("/api/v1/basket")
    .WithTags("Basket API")
    .WithOpenApi()
    .RegisterEndpoints();

app.Run();

namespace Basket.API
{
    public class Program
    {
    }
}