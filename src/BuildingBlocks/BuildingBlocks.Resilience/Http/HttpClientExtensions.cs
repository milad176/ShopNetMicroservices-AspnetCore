using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Resilience.Http;

public static class HttpClientExtensions

{
    public static IHttpClientBuilder AddStandardResiliencePolicies(this IHttpClientBuilder builder)
    {
        return builder
            .AddPolicyHandler(HttpRetryPolicies.RetryPolicy())
            .AddPolicyHandler(HttpRetryPolicies.CircuitBreakerPolicy())
            .AddPolicyHandler(HttpRetryPolicies.TimeoutPolicy());
    }
}