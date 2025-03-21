using System.ComponentModel.DataAnnotations;

namespace Products.Domain.Entities
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		
		public Product (int id, string name, double price, string description)
		{
			Id = id;
			Name = name;
			Price = price;
			Description = description;
		}
	}
}
