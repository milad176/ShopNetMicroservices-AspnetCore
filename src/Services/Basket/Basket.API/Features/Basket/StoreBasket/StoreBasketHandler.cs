using BuildingBlocks.CQRS.Command;

namespace Basket.API.Features.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart? ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(ShoppingCart ShoppingCart);

    public class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var result = await basketRepository.StoreBasketAsync(command.ShoppingCart!, cancellationToken).ConfigureAwait(false);
            return new StoreBasketResult(result);
        }
    }
}
