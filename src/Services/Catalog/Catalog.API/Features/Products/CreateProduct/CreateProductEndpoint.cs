namespace Catalog.API.Features.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static IEndpointRouteBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", CreateProduct)
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
        return app;
    }

    public static async Task<Created<CreateProductResponse>> CreateProduct(CreateProductRequest request, ISender sender)
    {
        var command = request.Adapt<CreateProductCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<CreateProductResponse>();
        return TypedResults.Created($"/api/v1/catalog/products/{response.Id}", response);
    }
}
