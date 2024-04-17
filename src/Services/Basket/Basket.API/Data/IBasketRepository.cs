namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken token = default);
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken token = default);
    Task<bool> DeleteBasketAsync(string userName, CancellationToken token = default);
}