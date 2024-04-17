namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken token = default)
    {
        return await session.LoadAsync<ShoppingCart>(userName, token);
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken token = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(token);
        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken token = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(token);
        return true;
    }
}