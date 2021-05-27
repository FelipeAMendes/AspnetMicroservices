﻿using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Catalog.Api.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext _catalogContext;

		public ProductRepository(ICatalogContext catalogContext)
		{
			_catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
		}

		public async Task<IEnumerable<Product>> GetAll()
		{
			return await _catalogContext
				.Products
				.Find(p => true)
				.ToListAsync();
		}

		public async Task<Product> GetById(string id)
		{
			return await _catalogContext
				.Products
				.Find(p => p.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetByName(string name)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

			return await _catalogContext
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetByCategory(string category)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

			return await _catalogContext
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task Create(Product product)
		{
			await _catalogContext.Products.InsertOneAsync(product);
		}

		public async Task<bool> Update(Product product)
		{
			var updatedProduct = await _catalogContext
				.Products
				.ReplaceOneAsync(
					filter: f => f.Id == product.Id,
					replacement: product
				);

			return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
		}

		public async Task<bool> Delete(string id)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

			DeleteResult deleteResult = await _catalogContext
				.Products
				.DeleteOneAsync(filter);

			return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
		}
	}
}
