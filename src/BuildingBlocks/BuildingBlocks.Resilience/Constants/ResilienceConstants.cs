namespace BuildingBlocks.Resilience.Constants;

public static class ResilienceConstants

{
    public const int RetryCount = 3;

    public static readonly TimeSpan RetryBaseDelay = TimeSpan.FromSeconds(2);

    public static readonly TimeSpan CircuitBreakDuration = TimeSpan.FromSeconds(15);

    public static readonly TimeSpan TimeoutDuration = TimeSpan.FromSeconds(10);
}