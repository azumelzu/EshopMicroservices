namespace Basket.API.Basket.Commands.DeleteBasket;

public record DeleteBasketResopnse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            return Results.Ok(result.Adapt<DeleteBasketResopnse>());
        }).WithName("Delete Basket")
        .Produces<DeleteBasketResopnse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete basket")
        .WithDescription("Delete basket");
    }
}