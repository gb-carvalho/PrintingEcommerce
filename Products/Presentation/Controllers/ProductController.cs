using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOs;
using Products.Application.Services;
using System.Threading.Tasks;

namespace Products.Presentation.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{

		private readonly ProductService _productService;

		public ProductController(ProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			IEnumerable<ProductDto> products = await _productService.GetAllProductsAsync();
			return Ok(products);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
		{
			if (productDto == null)
			{
				return BadRequest("Produto inválido.");
			}

			await _productService.AddProductAsync(productDto);

			return Created("", productDto);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			ProductDto? product = await _productService.GetProductByIdAsync(id);

			if (product == null)
			{
				return BadRequest("Produto inválido.");
			}

			await _productService.DeleteProductAsync(id);
			return NoContent();
		}
	}
}