using Basket.API.Basket.Queries.GetBasket;

namespace Basket.API.Basket.Commands.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket", async (StoreBasketRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<StoreBasketCommand>());
            return Results.Created($"/basket/{result.UserName}", result.Adapt<StoreBasketResponse>());
        })
        .WithName("StoreBasket")
        .Produces<StoreBasketResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store basket")
        .WithDescription("Store basket");
    }
}