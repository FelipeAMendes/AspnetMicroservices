using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : Controller
	{
		private readonly IBasketRepository _basketRepository;

		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
		}

		/// <summary>
		/// Get Basket from User
		/// </summary>
		/// <param name="userName">UserName key</param>
		/// <returns>User's Shopping Cart</returns>
		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _basketRepository.Get(userName);
			return Ok(basket ?? new ShoppingCart(userName));
		}

		/// <summary>
		/// Insert or Update Basket
		/// </summary>
		/// <param name="shoppingCart">Shopping Cart data</param>
		/// <returns>ShoppingCart data inserted or updated</returns>
		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
		{
			return Ok(await _basketRepository.Update(shoppingCart));
		}

		/// <summary>
		/// Delete Basket by userName key
		/// </summary>
		/// <param name="userName">userName key</param>
		/// <returns>Http Status Code OK</returns>
		[HttpDelete("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await _basketRepository.Delete(userName);
			return Ok();
		}
	}
}
