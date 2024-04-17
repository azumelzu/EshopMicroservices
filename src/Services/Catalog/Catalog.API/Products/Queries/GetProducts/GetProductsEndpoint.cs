namespace Catalog.API.Products.Queries.GetProducts;
public record GetProductsRequest(int? PageNumber, int? PageSize);
public record GetProductsResponse( IEnumerable<Product> Products );

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) => 
            {
                var result = await sender.Send(request.Adapt<GetProductsQuery>());
                return Results.Ok( result.Adapt<GetProductsResponse>());
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products")
            .WithDescription("Get all products");
    }
}