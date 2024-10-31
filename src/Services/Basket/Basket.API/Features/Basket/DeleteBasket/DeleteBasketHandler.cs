using BuildingBlocks.CQRS.Command;

namespace Basket.API.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        //TODO: delete basket from database and cache
        //sessoin.Delete<Product>(command.Id);

        return new DeleteBasketResult(true);
    }
}
