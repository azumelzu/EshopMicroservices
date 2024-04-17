using Marten.Pagination;

namespace Catalog.API.Products.Queries.GetProducts;
internal record GetProductsQuery(int? PageNumber, int? PageSize) : IQuery<GetProductsResult>;
internal record GetProductsResult( IEnumerable<Product> Products);

internal class GetProductsQueryHandler (IDocumentSession session) 
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productsResult = await session
            .Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
       
        return new GetProductsResult(productsResult);
    }
}