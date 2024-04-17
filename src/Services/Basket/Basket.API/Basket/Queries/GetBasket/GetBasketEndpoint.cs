namespace Basket.API.Basket.Queries.GetBasket;

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            return Results.Ok(result.Adapt<GetBasketResponse>());
        })
        .WithName("GetBasket")
        .Produces<GetBasketResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get basket")
        .WithDescription("Get user basket");
    }
}