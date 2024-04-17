using Marten.Pagination;

namespace Catalog.API.Products.Queries.GetProductByCategory;
internal record GetProductByCategoryQuery(string Category, int? PageNumber, int? PageSize) : IQuery<GetProductByCategoryResult>;
internal record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler (IDocumentSession session) 
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var productsResult = await session.Query<Product>()
            .Where(x => x.Category.Contains(query.Category))
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductByCategoryResult(productsResult);
    }
}