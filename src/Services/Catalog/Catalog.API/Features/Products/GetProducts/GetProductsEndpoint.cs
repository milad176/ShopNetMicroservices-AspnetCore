namespace Catalog.API.Features.Products.GetProducts
{
    public static class GetProductsEndpoint
    {
        public static IEndpointRouteBuilder MapGetProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/products", GetProducts)
                .WithName("GetProducts")
                .Produces<GetProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products")
                .WithDescription("Get Products");
            return app;
        }

        private static async Task<Ok<GetProductResponse>> GetProducts(ISender sender)
        {
            var result = await sender.Send(new GetProductsQuery());
            return result.Adapt<Ok<GetProductResponse>>();
        }
    }
}
