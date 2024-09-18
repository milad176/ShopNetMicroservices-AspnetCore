using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.CQRS.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Start handling Request={Request} - response={Response} - RequestData={RequestData}",
                        typeof(TRequest).Name, typeof(TResponse).Name, request);
            return await next();
        }
    }
}
