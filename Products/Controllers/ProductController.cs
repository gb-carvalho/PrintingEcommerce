using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Models;
using System.Threading.Tasks;

namespace Products.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{

		private readonly ApplicationDbContext _context;

		public ProductController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var products = await _context.Products.ToListAsync();
			return Ok(products);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateProduct([FromBody] Product product)
		{
			if (product == null)
			{
				return BadRequest("Produto inválido.");
			}

			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			return Created("", product);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			Product? product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				return BadRequest("Produto inválido.");
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}