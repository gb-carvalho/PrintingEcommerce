using System.ComponentModel.DataAnnotations;

namespace Products.Application.DTOs
{
	public class ProductDto
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public double Price { get; set; }
		public string Description { get; set; }

		public ProductDto(int id, string name, double price, string description)
		{
			Id = id;
			Name = name;
			Price = price;
			Description = description;
		}
	}
}
