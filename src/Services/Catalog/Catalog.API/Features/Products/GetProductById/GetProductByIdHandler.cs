namespace Catalog.API.Features.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);

    internal class GetProductByIQueryHandler(
        IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken).ConfigureAwait(false);

            if (product is null)
                throw new ProductNotFoundException(query.Id);

            return new GetProductByIdResult(product);
        }
    }
}
