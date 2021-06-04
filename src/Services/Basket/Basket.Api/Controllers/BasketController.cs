using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IBasketRepository _basketRepository;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly DiscountGrpcService _discountGrpcService;

		public BasketController(
			IMapper mapper,
			IBasketRepository basketRepository,
			IPublishEndpoint publishEndpoint,
			DiscountGrpcService discountGrpcService)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
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

		/// <summary>
		/// Basket Checkout
		/// </summary>
		/// <param name="basketCheckout">basket checkout object</param>
		/// <returns>Http Status Code Accepted or BadRequest</returns>
		[Route("[action]")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
		{
			// 1. get existing basket with total price            
			// 2. Create BasketCheckoutEvent - Set TotalPrice on basketCheckout eventMessage
			// 3. send checkout event to rabbitmq
			// 4. remove the basket

			// 1.
			var basket = await _basketRepository.Get(basketCheckout.UserName);
			if (basket == null)
			{
				return BadRequest();
			}

			// send checkout event to rabbitmq
			var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
			eventMessage.TotalPrice = basket.TotalPrice;
			await _publishEndpoint.Publish<BasketCheckoutEvent>(eventMessage);

			// remove the basket
			await _basketRepository.Delete(basket.UserName);

			return Accepted();
		}
	}
}
