using Products.Domain.Entities;

namespace Products.Domain.Interfaces
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetAllAsync();
		Task AddAsync(Product product);
		Task DeleteAsync(int id);
		Task<Product?> GetByIdAsync(int id);

	}
}
