namespace Catalog.API.Products.Queries.GetProductById;

// public record GetProductByIdRequest (Guid Id);
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return Results.Ok(result.Adapt<GetProductByIdResponse>());
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get product by id")
        .WithDescription("Get product by id");
    }
}