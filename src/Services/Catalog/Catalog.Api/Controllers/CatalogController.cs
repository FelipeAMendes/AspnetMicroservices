using Catalog.Api.Entities;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatalogController : Controller
	{
		private readonly ILogger<CatalogController> _logger;
		private readonly IProductRepository _productRepository;

		public CatalogController(
			ILogger<CatalogController> logger,
			IProductRepository productRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
		}

		/// <summary>
		/// Get All products from MongoDB
		/// </summary>
		/// <returns>List products</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productRepository.GetAll();
			return Ok(products);
		}

		/// <summary>
		/// Get product by id from MongoDB
		/// </summary>
		/// <returns>Product found</returns>
		[HttpGet("{id:length(24)}", Name = "GetProduct")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> GetProductById(string id)
		{
			var product = await _productRepository.GetById(id);
			if (product is null)
			{
				_logger.LogError($"Product with id {id} was not found!");
				return NotFound();
			}

			return Ok(product);
		}

		/// <summary>
		/// Get product by name from MongoDB
		/// </summary>
		/// <returns>Product found</returns>
		[Route("[action]/{name}", Name = "GetProductByName")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
		{
			var products = await _productRepository.GetByName(name);
			return Ok(products);
		}

		/// <summary>
		/// Get product by category from MongoDB
		/// </summary>
		/// <returns>Product found</returns>
		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
		{
			var products = await _productRepository.GetByCategory(category);
			return Ok(products);
		}

		/// <summary>
		/// Post product to MongoDB
		/// </summary>
		/// <param name="product">Product to save</param>
		/// <returns>Product created on route</returns>
		[HttpPost]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _productRepository.Create(product);

			return CreatedAtRoute("Get", new { id = product.Id }, product);
		}

		/// <summary>
		/// Update product to MongoDB
		/// </summary>
		/// <param name="product">Product to update</param>
		/// <returns>true or false</returns>
		[HttpPut]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			return Ok(await _productRepository.Update(product));
		}

		/// <summary>
		/// Delete product to MongoDB
		/// </summary>
		/// <param name="id">Product id to delete</param>
		/// <returns>true or false</returns>
		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteProductById(string id)
		{
			return Ok(await _productRepository.Delete(id));
		}
	}
}
