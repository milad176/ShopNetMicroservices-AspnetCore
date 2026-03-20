using Refit;
using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Services;

public interface ICatalogService
{
    [Get("/catalog-service/api/v1/catalog/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

    [Get("/catalog-service/api/v1/catalog/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);

    [Get("/catalog-service/api/v1/catalog/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);
}