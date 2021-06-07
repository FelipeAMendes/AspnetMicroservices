using AspNetWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetWebApp.Services
{
	public interface IOrderService
	{
		Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
	}
}
