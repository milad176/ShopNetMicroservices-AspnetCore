using Basket.API.Common;
using BuildingBlocks.Exceptions.Handler;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterMediateR(typeof(Program).Assembly);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
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