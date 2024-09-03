namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
Guid? Id,
string Name,
List<string> Category,
string? Description,
string ImageFile,
decimal? Price) : ICommand<CreateProductResult>;

public record CreateProductResult(
    Guid Id,
    string Name,
    List<string> Category,
    string? Description,
    string ImageFile,
    decimal? Price);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = command.Id ?? default,
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        var result = product.Adapt<CreateProductResult>();

        return result;
    }
}
