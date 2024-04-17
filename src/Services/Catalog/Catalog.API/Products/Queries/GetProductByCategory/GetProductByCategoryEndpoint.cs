namespace Catalog.API.Products.Queries.GetProductByCategory;

public record GetProductByCategoryRequest(string Category, int? PageNumber, int? PageSize);
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async ([AsParameters] GetProductByCategoryRequest request, ISender sender) =>
            {
                var result = await sender.Send(request.Adapt<GetProductByCategoryQuery>());
                return Results.Ok(result.Adapt<GetProductByCategoryResponse>());
            })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get product by category")
        .WithDescription("Get product by category");
    }
}