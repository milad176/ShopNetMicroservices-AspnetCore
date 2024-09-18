using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProducts
{
    public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;
    public record GetProductsResult(PaginatedItems<ProductModule> Product);

    internal class GetProductsQueryHandler(
    IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var pageSize = query.PaginationRequest.PageSize;
            var pageIndex = query.PaginationRequest.PageIndex;
            var productsAsQueryable = session.Query<Product>().AsQueryable();
            var totalItems = await productsAsQueryable.CountAsync(cancellationToken).ConfigureAwait(false);
            var products = await productsAsQueryable.Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var result = products.Adapt<IEnumerable<ProductModule>>();

            return new GetProductsResult(new PaginatedItems<ProductModule>(pageIndex, pageSize, totalItems, result));
        }
    }
}
