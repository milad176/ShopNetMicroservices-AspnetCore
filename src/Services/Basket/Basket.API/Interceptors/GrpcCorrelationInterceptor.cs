using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Basket.API.Interceptors;

public class GrpcCorrelationInterceptor : Interceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GrpcCorrelationInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier;

        var headers = context.Options.Headers ?? new Metadata();

        if (!string.IsNullOrEmpty(correlationId) && !headers.Any(h => h.Key == "x-correlation-id"))
        {
            headers.Add("x-correlation-id", correlationId);
        }

        var newOptions = context.Options.WithHeaders(headers);

        var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);

        return continuation(request, newContext);
    }
}