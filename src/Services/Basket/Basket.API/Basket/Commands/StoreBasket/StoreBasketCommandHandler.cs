using Basket.API.Data;
using static Discount.Grpc.DiscountProtoService;
using Discount.Grpc;

namespace Basket.API.Basket.Commands.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreCommandValidator()
    {
        RuleFor(x => x.Cart)
            .NotNull()
            .WithMessage("Cart can not be null");

        RuleFor(x => x.Cart.UserName)
            .NotEmpty()
            .WithMessage("User name can not be empty");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoServiceClient discountProtoServiceClient): ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart.Items, cancellationToken);
        var basket = await repository.StoreBasketAsync(command.Cart, cancellationToken);
        return new StoreBasketResult(basket.UserName);
    }

    private async Task DeductDiscount(List<ShoppingCartItem> items, CancellationToken cancellationToken)
    {
        // TODO: Make one call that returns a list of discounts to avoid multiple calls to the service
        foreach (var i in items)
        {
            var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = i.ProductName }, cancellationToken: cancellationToken);
            i.Price -= coupon.Amount;
        }
    }
}