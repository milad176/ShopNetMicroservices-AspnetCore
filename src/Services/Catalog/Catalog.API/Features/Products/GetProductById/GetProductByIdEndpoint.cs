namespace Catalog.API.Features.Products.GetProductById;

public static class GetProductByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetProductByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", GetProductById)
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product by id")
            .WithDescription("Get Product by id");

        return app;
    }

    private static async Task<Ok<GetProductByIdResponse>> GetProductById(Guid id, ISender sender)
    {
        var queryResult = await sender.Send(new GetProductByIdQuery(id)).ConfigureAwait(false);
        var result = queryResult.Product.Adapt<GetProductByIdResponse>();
        return TypedResults.Ok(result);
    }
}

