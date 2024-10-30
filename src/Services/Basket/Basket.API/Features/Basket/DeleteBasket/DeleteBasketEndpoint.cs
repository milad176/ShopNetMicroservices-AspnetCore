using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Features.Basket.DeleteBasket
{
    public static class DeleteBasketEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{username}", DeleteBasketAsync)
                .WithName("DeleteBasket")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Basket by username")
                .WithDescription("Delete Product by username");

            return app;
        }

        private static async Task<Ok<DeleteBasketResponse>> DeleteBasketAsync(string username, ISender sender)
        {
            var queryResult = await sender.Send(new DeleteBasketCommand(username)).ConfigureAwait(false);
            var result = queryResult.Adapt<DeleteBasketResponse>();
            return TypedResults.Ok(result);
        }
    }
}
