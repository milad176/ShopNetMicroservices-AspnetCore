using Basket.API.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterMediateR(typeof(Program).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
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