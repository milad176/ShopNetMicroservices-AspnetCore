using BuildingBlocks.CQRS.Command;

namespace Basket.API.Features.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart? ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(ShoppingCart ShoppingCart);

    public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart shoppingCart = command.ShoppingCart;

            //TODO: store basket in database (use Marten upsert - if exist = update, if not create)
            //TODO: Update cache

            return new StoreBasketResult(new ShoppingCart("swn"));
        }
    }
}
