using Basket.API.Features.Basket.DeleteBasket;
using Basket.API.Features.Basket.GetBasket;
using Basket.API.Features.Basket.StoreBasket;

namespace Basket.API.Common;

public static class RoutingRegistrar
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetBasketEndpoint();
        app.MapStoreBasketEndpoint();
        app.MapDeleteBasketEndpoint();
    }
}
