using Catalog.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories.Interfaces
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetAll();
		Task<Product> GetById(string id);
		Task<IEnumerable<Product>> GetByName(string name);
		Task<IEnumerable<Product>> GetByCategory(string category);

		Task Create(Product product);
		Task<bool> Update(Product product);
		Task<bool> Delete(string id);
	}
}
