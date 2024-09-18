namespace Catalog.API.Features.Products.GetProductsByCategory
{
    public record GetProductByCategoryResponse(PaginatedItems<ProductModule> Product);
}
