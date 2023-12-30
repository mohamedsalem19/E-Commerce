using ECommerce.Core.Entities;
using ECommerce.Core.IRepositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;   

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketASync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            return  basket.IsNull ?  null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var updatedOrCreatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));

            if (updatedOrCreatedBasket is false) return null;

            return await GetBasketASync(basket.Id);

        }
    }
}
