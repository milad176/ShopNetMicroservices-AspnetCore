using BuildingBlocks.HealthChecks;
using BuildingBlocks.OpenTelemetry;
using Common.Logging;
using Discount.Grpc.Data;
using Discount.Grpc.Interceptors;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;
using Discount.Grpc.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSeriLogging();

// Add services to the container.
builder.Services.AddSingleton<CorrelationIdInterceptor>();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CorrelationIdInterceptor>();
});

const string serviceName = "eshop.discount.grpc";
builder.Services.AddOpenTelemetryOtl(serviceName);

builder.Services.AddDbContext<DiscountContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddHealthChecks(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigration();
app.MapGrpcService<DiscountService>();
app.MapDefaultHealthChecks();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();