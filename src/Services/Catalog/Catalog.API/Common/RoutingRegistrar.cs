using Catalog.API.Features.Products.CreateProduct;
using Catalog.API.Features.Products.GetProductById;
using Catalog.API.Features.Products.GetProducts;
using Catalog.API.Features.Products.GetProductsByCategory;

namespace Catalog.API.Common
{
    public static class RoutingRegistrar
    {
        public static void RegisterEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapCreateProductEndpoint();
            app.MapGetProductEndpoint();
            app.MapGetProductByIdEndpoint();
            app.MapGetProductByCategoryEndpoint();
        }
    }
}
