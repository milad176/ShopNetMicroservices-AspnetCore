using BuildingBlocks.Authentication;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddBuildingBlocksAuthentication(builder.Configuration);

// Keep Entra claim names as they appear in the JWT: tid, oid, sub, iss, scp.
builder.Services.PostConfigure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options => options.MapInboundClaims = false);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IdentityRead", policy =>
        policy.RequireAuthenticatedUser()
            .RequireAssertion(context =>
                context.User.FindAll("scp")
                    .SelectMany(claim => claim.Value.Split(
                        ' ',
                        StringSplitOptions.RemoveEmptyEntries))
                    .Contains("identity.read", StringComparer.Ordinal)));
});

var connectionString = builder.Configuration.GetConnectionString("Database")
                       ?? throw new InvalidOperationException(
                           "Connection string 'Database' is not configured.");

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/v1/users/me", async (
    System.Security.Claims.ClaimsPrincipal principal,
    IdentityDbContext db,
    CancellationToken cancellationToken) =>
{
    var tenantId = principal.FindFirst("tid")?.Value;
    var objectId = principal.FindFirst("oid")?.Value;
    var subject = principal.FindFirst("sub")?.Value;
    var issuer = principal.FindFirst("iss")?.Value;

    if (string.IsNullOrWhiteSpace(tenantId)
        || string.IsNullOrWhiteSpace(objectId)
        || string.IsNullOrWhiteSpace(subject)
        || string.IsNullOrWhiteSpace(issuer))
    {
        return Results.Forbid();
    }

    var user = await db.ApplicationUsers.SingleOrDefaultAsync(
        x => x.TenantId == tenantId && x.ObjectId == objectId,
        cancellationToken);

    if (user is null)
    {
        user = new ApplicationUser
        {
            TenantId = tenantId,
            ObjectId = objectId,
            Issuer = issuer,
            Subject = subject
        };

        db.ApplicationUsers.Add(user);
        await db.SaveChangesAsync(cancellationToken);
    }

    return Results.Ok(new
    {
        user.Id,
        user.CreatedAtUtc
    });
}).RequireAuthorization("IdentityRead");

app.Run();
