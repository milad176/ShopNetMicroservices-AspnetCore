using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSeriLogging();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); });
builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); });
builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); });

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();