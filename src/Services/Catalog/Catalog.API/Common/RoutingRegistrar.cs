using Catalog.API.Features.Products.CreateProduct;

namespace Catalog.API.Common
{
    public static class RoutingRegistrar
    {
        public static void RegisterEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapCreateProductEndpoint();
        }
    }
}
