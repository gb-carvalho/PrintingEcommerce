using Microsoft.EntityFrameworkCore;
using Products.Models;


namespace Products.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

		public DbSet<Product> Products { get; set; }
	}
}
