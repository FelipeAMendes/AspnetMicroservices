using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ShoppingController : ControllerBase
	{
		private readonly ICatalogService _catalogService;
		private readonly IBasketService _basketService;
		private readonly IOrderService _orderService;

		public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
		{
			_catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
			_basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
			_orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
		}

		[HttpGet("{userName}", Name = "GetShopping")]
		[ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
		{
			// 1. Get basket by username
			// 2. Iterate basket items and consume products with basket item productId member
			// 3. Map product related members in basketitem DTO with extended columns
			// 4. Consume order microservices to retrieve the order list
			// 5. Return the root ShoppngModel DTO class that includes all responses

			var basket = await _basketService.GetBasket(userName);

			foreach (var item in basket.Items)
			{
				var product = await _catalogService.GetCatalog(item.ProductId);

				// Set additional product fields onto basket item
				item.ProductName = product.Name;
				item.Category = product.Category;
				item.Summary = product.Summary;
				item.Description = product.Description;
				item.ImageFile = product.ImageFile;
			}

			var orders = await _orderService.GetOrdersByUserName(userName);

			var shoppingModel = new ShoppingModel
			{
				UserName = userName,
				BasketWithProducts = basket,
				Orders = orders
			};

			return Ok(shoppingModel);
		}
	}
}
