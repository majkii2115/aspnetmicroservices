﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;
public class BasketRepository : IBasketRepository
{

    private readonly IDistributedCache cache;

    public BasketRepository(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task DeleteBasket(string userName)
    {
        await cache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var basket = await cache.GetStringAsync(userName);
        if (String.IsNullOrEmpty(basket))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        await cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

        return await GetBasket(basket.UserName);
    }
}
