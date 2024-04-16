using MediatR;

namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductCommand(
Guid Id,
string Name,
List<string> Category,
string? Description,
string ImageFile,
decimal? Price) : IRequest<CreateProductResult>;

public record CreateProductResult(
    Guid Id,
    string Name,
    List<string> Category,
    string? Description,
    string ImageFile,
    decimal? Price);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
