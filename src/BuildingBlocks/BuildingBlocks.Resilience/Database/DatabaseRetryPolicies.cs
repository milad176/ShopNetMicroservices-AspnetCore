using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Resilience.Database;

public static class DatabaseRetryPolicies
{
    public static AsyncPolicy CreateRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<SqlException>()
            .Or<SocketException>()
            .Or<TimeoutException>()
            .Or<DbUpdateException>()
            .WaitAndRetryAsync(
                5,
                retry =>
                    TimeSpan.FromSeconds(Math.Pow(2, retry)),
                onRetry: (exception, timespan, retryCount, _) =>
                {
                    logger.LogWarning(
                        exception,
                        "DB Retry {RetryCount} after {Delay}s",
                        retryCount,
                        timespan.TotalSeconds);
                });
    }
}