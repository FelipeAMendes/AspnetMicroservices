using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _distributedCache;

		public BasketRepository(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(_distributedCache));
		}

		public async Task<ShoppingCart> Get(string userName)
		{
			var basket = await _distributedCache.GetStringAsync(userName);
			return string.IsNullOrEmpty(basket)
				? null
				: JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart> Update(ShoppingCart shoppingCart)
		{
			await _distributedCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
			
			return await Get(shoppingCart.UserName);
		}

		public async Task Delete(string userName)
		{
			await _distributedCache.RemoveAsync(userName);
		}
	}
}
