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

        private static async Task<Ok<GetProductsResponse>> GetProducts(
            [AsParameters] PaginationRequest paginationRequest,
            ISender sender)
        {
            var query = new GetProductsQuery(paginationRequest);
            var queryResult = await sender.Send(query).ConfigureAwait(false);

            var response = new GetProductsResponse(queryResult.Product);

            return TypedResults.Ok(response);
        }
    }
}
