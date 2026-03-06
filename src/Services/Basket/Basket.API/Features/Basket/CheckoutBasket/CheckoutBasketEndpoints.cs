using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Features.Basket.CheckoutBasket;

public static class CheckoutBasketEndpoints
{
    public static IEndpointRouteBuilder MapCheckoutBasketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", CheckoutBasketAsync)
            .WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Checkout Basket")
            .WithDescription("Checkout Basket");
        return app;
    }

    private static async Task<Ok<CheckoutBasketResponse>> CheckoutBasketAsync(CheckoutBasketRequest request,
        ISender sender)
    {
        var command = request.Adapt<CheckoutBasketCommand>();

        var result = await sender.Send(command);

        var response = result.Adapt<CheckoutBasketResponse>();
        return TypedResults.Ok(response);
    }
}