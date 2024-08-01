namespace Catalog.API.Features.Products.GetProducts
{
    public static class GetProductsEndpoint
    {
        public static IEndpointRouteBuilder MapGetProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/products", GetProducts)
                .WithName("GetProducts")
                .Produces<GetProductsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products")
                .WithDescription("Get Products");
            return app;
        }

        private static async Task<Ok<GetProductsResponse>> GetProducts(ISender sender)
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = new GetProductsResponse(result.Products);

            return TypedResults.Ok(response);
        }
    }
}
