
namespace Catalog.API.Features.Products.UpdateProduct
{
    public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string>? Category,
    string? Description,
    string? ImageFile,
    decimal? Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(ProductModule Product);

    internal class UpdateProductsCommandHandler(
        IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken).ConfigureAwait(false);

            if (product is null)
                throw new ProductNotFoundException(command.Id);

            if (!string.IsNullOrEmpty(command.Name))
                product.Name = command.Name;
            if (command.Category?.Count != 0)
                product.Category = command.Category!;
            if (!string.IsNullOrEmpty(command.Description))
                product.Description = command.Description;
            if (!string.IsNullOrEmpty(command.ImageFile))
                product.ImageFile = command.ImageFile;

            product.Price = command.Price ?? product.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var updatedEntity = product.Adapt<ProductModule>();

            return new UpdateProductResult(updatedEntity);
        }
    }
}
