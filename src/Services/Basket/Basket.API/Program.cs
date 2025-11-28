using Basket.API.Common;
using Basket.API.Models.Configs;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.HealthChecks;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;
using Shared;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
});

builder.Services.AddHealthChecks(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseProblemDetailsResponseExceptionHandler();

app.Run();

namespace Basket.API
{
    public class Program
    {
    }
}