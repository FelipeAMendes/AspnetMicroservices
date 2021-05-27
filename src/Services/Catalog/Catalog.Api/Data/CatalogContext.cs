using Catalog.Api.Configs;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
	public class CatalogContext : ICatalogContext
	{
		public CatalogContext(IDatabaseSettings settings)
		{
			Products = new MongoClient(settings.ConnectionString)
				.GetDatabase(settings.DatabaseName)
				.GetCollection<Product>(settings.CollectionName);

			CatalogContextSeed.SeedData(Products);
		}

		public IMongoCollection<Product> Products { get; }
	}
}
