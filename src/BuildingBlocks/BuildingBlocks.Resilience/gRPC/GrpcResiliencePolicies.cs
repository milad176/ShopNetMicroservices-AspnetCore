using System.Net.Sockets;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Polly;

namespace BuildingBlocks.Resilience.gRPC;

public static class GrpcResiliencePolicies
{
    public static IAsyncPolicy<HttpResponseMessage> CreateGrpcRetryPolicy(ILogger? logger = null)
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode == 503)
            .Or<SocketException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retry => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retry)),
                onRetry: (outcome, delay, retryCount, _) =>
                {
                    logger?.LogWarning(
                        "gRPC retry {RetryCount} after {Delay}ms due to {Reason}",
                        retryCount,
                        delay.TotalMilliseconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });
    }
}