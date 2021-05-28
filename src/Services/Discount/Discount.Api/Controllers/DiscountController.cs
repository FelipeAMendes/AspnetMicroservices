using Discount.Api.Entities;
using Discount.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DiscountController : Controller
	{
		private readonly IDiscountRepository _discountRepository;

		public DiscountController(IDiscountRepository discountRepository)
		{
			_discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
		}

		/// <summary>
		/// Get Discount from Product
		/// </summary>
		/// <param name="productName">Product Name</param>
		/// <returns>discount object</returns>
		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> GetDiscount(string productName)
		{
			var discount = await _discountRepository.Get(productName);
			return Ok(discount);
		}

		/// <summary>
		/// Create new Discount
		/// </summary>
		/// <param name="coupon">discount object</param>
		/// <returns>object created</returns>
		[HttpPost]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
		{
			await _discountRepository.Create(coupon);
			return CreatedAtAction("GetDiscount", new { productName = coupon.ProductName }, coupon);
		}

		/// <summary>
		/// Update Discount
		/// </summary>
		/// <param name="coupon">discount object</param>
		/// <returns>true or false</returns>
		[HttpPut]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> UpdateBasket([FromBody] Coupon coupon)
		{
			return Ok(await _discountRepository.Update(coupon));
		}

		/// <summary>
		/// Delete an Discount object
		/// </summary>
		/// <param name="productName">discount object</param>
		/// <returns>true or false</returns>
		[HttpDelete("{productName}", Name = "DeleteDiscount")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteDiscount(string productName)
		{
			return Ok(await _discountRepository.Delete(productName));
		}
	}
}
