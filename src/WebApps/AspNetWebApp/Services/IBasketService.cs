using AspNetWebApp.Models;
using System.Threading.Tasks;

namespace AspNetWebApp.Services
{
	public interface IBasketService
	{
		Task<BasketModel> GetBasket(string userName);
		Task<BasketModel> UpdateBasket(BasketModel model);
		Task CheckoutBasket(BasketCheckoutModel model);
	}
}
