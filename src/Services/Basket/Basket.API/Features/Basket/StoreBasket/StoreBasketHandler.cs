using BuildingBlocks.CQRS.Command;

namespace Basket.API.Features.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart? ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(ShoppingCart ShoppingCart);

    public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
