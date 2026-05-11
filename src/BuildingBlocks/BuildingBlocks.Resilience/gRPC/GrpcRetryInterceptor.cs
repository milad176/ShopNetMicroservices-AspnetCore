using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net.Sockets;

namespace BuildingBlocks.Resilience.gRPC;

public class GrpcRetryInterceptor(ILogger<GrpcRetryInterceptor> logger) : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var responseAsync = Policy
            .Handle<RpcException>(ex =>
                ex.StatusCode == StatusCode.Unavailable ||
                ex.StatusCode == StatusCode.DeadlineExceeded)
            .Or<SocketException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retry =>
                    TimeSpan.FromSeconds(Math.Pow(2, retry)),
                onRetry: (exception, delay, retryCount, _) =>
                {
                    logger.LogWarning(
                        exception,
                        "gRPC Retry {RetryCount} after {Delay}s",
                        retryCount,
                        delay.TotalSeconds);
                })
            .ExecuteAsync(async () =>
            {
                var call = continuation(request, context);

                return await call.ResponseAsync;
            });

        return new AsyncUnaryCall<TResponse>(
            responseAsync,
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => new Metadata(),
            () => { });
    }
}