using BuildingBlocks.CQRS.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.CQRS.Extensions;


public static class CqrsExtensions
{
    public static void RegisterMediateR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(CommandValidationBehaviors<,>));
            config.AddOpenBehavior(typeof(QueryValidationBehaviors<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
    }
}

