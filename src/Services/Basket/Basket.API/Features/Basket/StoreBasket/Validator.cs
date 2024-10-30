using FluentValidation;

namespace Basket.API.Features.Basket.StoreBasket;

public class StoreBasketCommandValidator: AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.ShoppingCart!.Username).NotEmpty().WithMessage("Username is required.")
            .When(x => x.ShoppingCart is not null);

        When(x => x.ShoppingCart is not null, () =>
        {
            RuleFor(x => x.ShoppingCart!.Items).NotEmpty().WithMessage("Shopping items cannot be null");
            RuleForEach(x => x.ShoppingCart!.Items)
                .SetValidator(new ShoppingCartItemValidator());
        });
    }

    private class ShoppingCartItemValidator : AbstractValidator<ShoppingCartItem>
    {
        public ShoppingCartItemValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}