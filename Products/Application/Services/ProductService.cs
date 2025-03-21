using Products.Domain.Interfaces;
using Products.Application.DTOs;
using Products.Domain.Entities;

namespace Products.Application.Services
{
	public class ProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
		{
			var products = await _productRepository.GetAllAsync();
			return products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Description));
		}

		public async Task<ProductDto?> GetProductByIdAsync(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			return product == null ? null : new ProductDto (product.Id, product.Name, product.Price, product.Description );
		}

		public async Task AddProductAsync(ProductDto productDto)
		{
			var product = new Product (0, productDto.Name, productDto.Price, productDto.Description);
			await _productRepository.AddAsync(product);
		}

		public async Task DeleteProductAsync(int id)
		{
			await _productRepository.DeleteAsync(id);
		}
	}
}
