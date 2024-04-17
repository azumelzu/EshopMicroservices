using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

// Decorator implementation
public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken token = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, token);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

        var basket = await repository.GetBasketAsync(userName, token);
        if (basket is not null)
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), token);
        
        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken token = default)
    {
        await repository.StoreBasketAsync(basket, token);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), token);
        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken token = default)
    {
        await cache.RemoveAsync(userName, token);
        return await repository.DeleteBasketAsync(userName, token);
    }
}