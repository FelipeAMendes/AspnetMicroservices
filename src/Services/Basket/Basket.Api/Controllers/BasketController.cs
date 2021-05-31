using Basket.Api.Entities;
using Basket.Api.GrpcServices;
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
		private readonly DiscountGrpcService _discountGrpcService;

		public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			_basketRepository = basketRepository;
			_discountGrpcService = discountGrpcService;
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
			foreach (var shoppingCartItem in shoppingCart.Items)
			{
				var coupon = await _discountGrpcService.GetDiscount(shoppingCartItem.ProductName);
				shoppingCartItem.Price -= coupon.Amount;
			}

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
