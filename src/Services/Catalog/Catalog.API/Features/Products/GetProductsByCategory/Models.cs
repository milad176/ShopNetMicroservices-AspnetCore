namespace Catalog.API.Features.Products.GetProductsByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> products);
}
