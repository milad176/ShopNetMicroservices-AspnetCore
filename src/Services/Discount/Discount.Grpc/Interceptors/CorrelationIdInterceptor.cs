using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog.Context;

namespace Discount.Grpc.Interceptors;

public class CorrelationIdInterceptor : Interceptor
{
    private const string HeaderName = "x-correlation-id";

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var correlationId =
            context.RequestHeaders.FirstOrDefault(h => h.Key == HeaderName)?.Value
            ?? Guid.NewGuid().ToString();

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return await continuation(request, context);
        }
    }
}