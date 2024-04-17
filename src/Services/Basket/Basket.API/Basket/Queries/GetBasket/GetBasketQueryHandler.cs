using Basket.API.Data;

namespace Basket.API.Basket.Queries.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler (IBasketRepository repository) 
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(query.UserName, cancellationToken);

        if (basket is null) throw new BasketNotFoundException(query.UserName);

        return new GetBasketResult(basket);
    }
}