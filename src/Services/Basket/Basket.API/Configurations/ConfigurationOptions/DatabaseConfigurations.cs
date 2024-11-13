using System.ComponentModel.DataAnnotations;

namespace Basket.API.Configurations.ConfigurationOptions;

internal sealed record DatabaseConfigurations
{
    [ConfigurationKeyName("ConnectionStrings:Database"), Required]
    public required string PostgresDb { get; init; }

    [ConfigurationKeyName("ConnectionStrings:Redis"), Required]
    public required string Redis { get; init; }
}
