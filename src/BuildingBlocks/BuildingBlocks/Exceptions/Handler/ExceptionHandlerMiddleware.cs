using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BuildingBlocks.Exceptions.Handler;

public static class ExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseProblemDetailsResponseExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exHandlerFeature is not null)
                {
                    await HandleException(context, exHandlerFeature);
                }
            });
        });

        return app;
    }

    private static async Task HandleException(HttpContext context, IExceptionHandlerFeature exHandlerFeature)
    {
        var error = exHandlerFeature.Error;
        var logger = Log.Logger.ForContext(typeof(ExceptionHandlerMiddleware));
        logger.Error("Error Message:{exceptionMessage} occurred at {Time}", error.Message, DateTimeOffset.UtcNow);

        var result = error switch
        {
            BadRequestException => new ProblemDetails()
            {
                Detail = error.Message,
                Title = error.GetType().Name,
                Status = StatusCodes.Status400BadRequest
            },
            NotFoundException => new ProblemDetails()
            {
                Detail = error.Message,
                Title = error.GetType().Name,
                Status = StatusCodes.Status404NotFound
            },
            InternalServerException => new ProblemDetails()
            {
                Detail = error.Message,
                Title = error.GetType().Name,
                Status = StatusCodes.Status500InternalServerError
            },
            ValidationException exception => new ProblemDetails()
            {
                Detail = exception.Message,
                Title = exception.GetType().Name,
                Status = StatusCodes.Status400BadRequest,
                Extensions = new Dictionary<string, object?>()
                {
                    { "ValidationErrors" , exception?.Errors.Select(x =>
                    new InvalidPropertyResponse(x.PropertyName, x.ErrorMessage)).ToArray() ?? []}
                }
            },
            BadHttpRequestException { StatusCode: 400 } err => new ProblemDetails
            {
                Detail = error.Message,
                Title = error.GetType().Name,
                Status = StatusCodes.Status400BadRequest,
                Extensions = err.InnerException != null ? new Dictionary<string, object?>()
                {
                    {"MoreInfo",  err.InnerException?.Message}
                } : []
            },
            _ => new ProblemDetails()
            {
                Detail = "InternalServerError",
                Title = "Something unexpected happened.",
                Status = StatusCodes.Status500InternalServerError
            }
        };

        result.Extensions.Add("traceId", context.TraceIdentifier);
        context.Response.StatusCode = result.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(result);
    }
}
