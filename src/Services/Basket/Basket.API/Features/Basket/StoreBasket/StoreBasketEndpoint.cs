using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Features.Basket.StoreBasket;

public static class StoreBasketEndpoint
{
    public static IEndpointRouteBuilder MapStoreBasketEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", StoreBasketAsync)
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket");

        return app;
    }

    private static async Task<Ok<StoreBasketResponse>> StoreBasketAsync(StoreBasketRequest request, ISender sender)
    {
        var command = request.Adapt<StoreBasketCommand>();
        var result = await sender.Send(command).ConfigureAwait(false);
        var response = result.Adapt<StoreBasketResponse>();
        return TypedResults.Ok(response);
    }
}
