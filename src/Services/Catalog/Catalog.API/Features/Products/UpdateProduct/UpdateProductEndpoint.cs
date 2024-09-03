namespace Catalog.API.Features.Products.UpdateProduct
{
    public static class UpdateProductEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/products", UpdataeProduct)
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update Product")
                .WithDescription("Update Product");
            return app;
        }

        private static async Task<Ok<UpdateProductResponse>> UpdataeProduct(UpdateProductRequest request, ISender sender)
        {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command).ConfigureAwait(false);
            return TypedResults.Ok(new UpdateProductResponse(result.Product));
        }
    }
}
