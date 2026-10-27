using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddBuildingBlocksAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Authentication:Authority"];

                var audiences = configuration
                    .GetSection("Authentication:Audiences")
                    .Get<string[]>();

                if (audiences is { Length: > 0 })
                {
                    options.TokenValidationParameters.ValidAudiences = audiences;
                }
                else
                {
                    options.Audience = configuration["Authentication:Audience"];
                }
            });

        services.AddAuthorization();

        return services;
    }
}