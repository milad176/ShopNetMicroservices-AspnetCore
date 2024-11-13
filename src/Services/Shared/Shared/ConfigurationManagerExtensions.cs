using Microsoft.Extensions.Configuration;
using ReHackt.Extensions.Options.Validation;

namespace Shared;

public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// https://github.com/LionelVallet/ReHackt.Extensions.Options.Validation/blob/main/src/DataAnnotationsValidateRecursiveOptions.cs
    /// </summary>
    /// <param name="configurationManager"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T TryGetValidatedOptions<T>(this ConfigurationManager configurationManager) where T : class
    {
        var options = configurationManager.Get<T>() ??
                      throw new InvalidOperationException($"Failed resolving {typeof(T).FullName}.");

        var validator = new DataAnnotationsValidateRecursiveOptions<T>(string.Empty);
        var validationResult = validator.Validate(string.Empty, options);

        if (validationResult.Succeeded) return options;

        throw new InvalidOperationException(
            $"Failed validating {typeof(T).FullName} due to following failures : {validationResult.FailureMessage}");
    }
}
