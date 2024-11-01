
namespace Basket.API.Data;

public class BasketRepository : IBasketRepository
{
    public Task<ShoppingCart> GetBasketAsync(string UserName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeleteBasketAsync(string UserName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
