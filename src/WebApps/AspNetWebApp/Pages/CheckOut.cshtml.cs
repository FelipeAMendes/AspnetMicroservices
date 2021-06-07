using AspNetWebApp.Models;
using AspNetWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AspNetWebApp
{
	public class CheckOutModel : PageModel
	{
		private readonly IBasketService _basketService;

		public CheckOutModel(IBasketService basketService)
		{
			_basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
		}

		[BindProperty]
		public BasketCheckoutModel Order { get; set; }

		public BasketModel Cart { get; set; } = new BasketModel();

		public async Task<IActionResult> OnGetAsync()
		{
			const string userName = "mendes";
			Cart = await _basketService.GetBasket(userName);
			return Page();
		}

		public async Task<IActionResult> OnPostCheckOutAsync()
		{
			const string userName = "mendes";
			Cart = await _basketService.GetBasket(userName);

			if (!ModelState.IsValid)
				return Page();

			Order.UserName = userName;
			Order.TotalPrice = Cart.TotalPrice;

			await _basketService.CheckoutBasket(Order);
			return RedirectToPage("Confirmation", "OrderSubmitted");
		}
	}
}
