using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Features.Basket.GetBasket;

public static class GetBasketEndpoint
{
    public static IEndpointRouteBuilder MapGetBasketEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{username}", GetBasketAsync)
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket by username")
            .WithDescription("Get Basket by username");

        return app;
    }

    private static async Task<Ok<GetBasketResponse>> GetBasketAsync(string username, ISender sender)
    {
        var queryResult = await sender.Send(new GetBasketQuery(username)).ConfigureAwait(false);
        var result = queryResult.Adapt<GetBasketResponse>();
        return TypedResults.Ok(result);
    }
}
