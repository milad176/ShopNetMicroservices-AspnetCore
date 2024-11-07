using BuildingBlocks.CQRS.Command;

namespace Basket.API.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await basketRepository.DeleteBasketAsync(command.Username, cancellationToken).ConfigureAwait(false);
        return new DeleteBasketResult(result);
    }
}
