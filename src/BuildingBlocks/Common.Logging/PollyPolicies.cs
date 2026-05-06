using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Common.Logging;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Serilog.Log.Warning(
                        "Polly Retry {RetryCount} after {Delay}s due to {Reason}",
                        retryCount,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());
                });
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                3,
                TimeSpan.FromSeconds(15),
                onBreak: (result, breakDelay) =>
                {
                    Serilog.Log.Error("Circuit breaker opened for {Delay}s due to {Reason}",
                        breakDelay.TotalSeconds,
                        result.Exception?.Message ?? result.Result.StatusCode.ToString());
                },
                onReset: () => { Serilog.Log.Information("Circuit breaker reset."); });
    }

    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(10);
    }
}