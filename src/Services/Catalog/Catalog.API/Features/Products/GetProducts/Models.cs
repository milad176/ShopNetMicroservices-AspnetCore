namespace Catalog.API.Features.Products.GetProducts
{
    public record GetProductsResponse(PaginatedItems<ProductModule> Product);
}
