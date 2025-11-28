using BuildingBlocks.CQRS.Command;
using Discount.Grpc.Protos;

namespace Basket.API.Features.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart? ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(ShoppingCart ShoppingCart);

    public class StoreBasketCommandHandler(
        IBasketRepository basketRepository,
        DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            if (command.ShoppingCart is null)
                throw new ArgumentNullException(nameof(command.ShoppingCart));
            
            await DeductDiscount(command.ShoppingCart, cancellationToken);

            var result = await basketRepository.StoreBasketAsync(command.ShoppingCart!, cancellationToken)
                .ConfigureAwait(false);
            return new StoreBasketResult(result);
        }

        private async Task DeductDiscount( ShoppingCart  shoppingCart , CancellationToken cancellationToken)
        {
            foreach (var item in  shoppingCart.Items)
            {
                var coupon = await discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken);

                item.Price -= coupon.Amount;
            }
        }
    }
}