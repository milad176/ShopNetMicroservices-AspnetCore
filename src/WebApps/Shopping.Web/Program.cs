using BuildingBlocks.HealthChecks;
using BuildingBlocks.OpenTelemetry;
using BuildingBlocks.Resilience.Http;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Serilog;
using Shopping.Web.Common;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "eshop.web";
builder.Services.AddOpenTelemetryOtl(serviceName);

// Serilog from shared library
builder.Host.UseSeriLogging();

// Add services to the container.
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Services.AddTransient<AccessTokenDelegatingHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.ClientId = builder.Configuration["Authentication:ClientId"]!;
        options.Authority = builder.Configuration["Authentication:Authority"]!;
        options.ClientSecret = builder.Configuration["Authentication:ClientSecret"]!;

        options.ResponseType = "code";
        options.SaveTokens = true;

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("api://aad4dd1c-0a4a-4b3b-b51b-f751248ae885/catalog.read");
    });

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks(builder.Configuration);

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); })
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddHttpMessageHandler<AccessTokenDelegatingHandler>()
    .AddStandardResiliencePolicies();

builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); })
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddHttpMessageHandler<AccessTokenDelegatingHandler>()
    .AddStandardResiliencePolicies();

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!); })
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddHttpMessageHandler<AccessTokenDelegatingHandler>()
    .AddStandardResiliencePolicies();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultHealthChecks();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();