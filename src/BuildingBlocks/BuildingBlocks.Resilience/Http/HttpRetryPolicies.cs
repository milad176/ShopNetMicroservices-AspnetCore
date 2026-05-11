using BuildingBlocks.Resilience.Constants;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace BuildingBlocks.Resilience.Http;

public static class HttpRetryPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                ResilienceConstants.RetryCount,
                retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(ResilienceConstants.RetryBaseDelay.TotalSeconds, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, _) =>
                {
                    Serilog.Log.Warning(
                        "HTTP Retry {RetryCount} after {Delay}s due to {Reason}",
                        retryCount,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());
                });
    }

    public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                3,
                ResilienceConstants.CircuitBreakDuration,
                onBreak: (result, duration) =>
                {
                    Serilog.Log.Error(
                        "Circuit breaker opened for {Duration}s due to {Reason}",
                        duration.TotalSeconds,
                        result.Exception?.Message ??
                        result.Result.StatusCode.ToString());
                },
                onReset: () => { Serilog.Log.Information("Circuit breaker reset."); });
    }

    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(ResilienceConstants.TimeoutDuration);
    }
}