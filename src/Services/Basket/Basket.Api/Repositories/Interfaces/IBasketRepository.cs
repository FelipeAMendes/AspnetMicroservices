using Basket.Api.Entities;
using System.Threading.Tasks;

namespace Basket.Api.Repositories.Interfaces
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> Get(string userName);
		Task<ShoppingCart> Update(ShoppingCart shoppingCart);
		Task Delete(string userName);
	}
}
