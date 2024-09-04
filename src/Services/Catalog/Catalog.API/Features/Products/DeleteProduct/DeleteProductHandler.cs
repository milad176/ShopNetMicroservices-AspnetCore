
namespace Catalog.API.Features.Products.DeleteProduct
{
    public record DeleteProductCommand(string? Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductCommandHandler(
        IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            session.Delete<Product>(Guid.Parse(command.Id!));
            await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new DeleteProductResult(true);
        }
    }
}
