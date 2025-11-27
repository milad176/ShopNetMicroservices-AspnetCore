using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Common;
using Catalog.API.Data;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddDefaultOpenApi();
builder.Services.RegisterMediateR(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<Product>().UseNumericRevisions(true);
}).UseLightweightSessions();

builder.Services.AddHealthChecks(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialDataMigration>();
}

var app = builder.Build();

// ✔️ Add routing BEFORE exception handler
app.UseRouting();

// ✔️ Exception handler AFTER routing, BEFORE endpoints
app.UseProblemDetailsResponseExceptionHandler();

// OpenAPI
app.UseDefaultOpenApi();

// Health checks
app.MapDefaultHealthChecks();

// Catalog endpoints
app.MapGroup("/api/v1/catalog")
    .WithTags("Catalog API")
    .RegisterEndpoints();

// Run
await app.RunAsync();

public partial class Program { }