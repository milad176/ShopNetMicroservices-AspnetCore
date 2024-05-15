using Catalog.API.Common;
using Shared;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddDefaultOpenApi();
builder.Services.RegisterMediateR(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultOpenApi();

app.MapGroup("/api/v1/catalog")
    .WithTags("Catalog API")
    .RegisterEndpoints();

await app.RunAsync();

public partial class Program
{
}