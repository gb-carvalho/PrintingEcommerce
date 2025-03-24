using System.ComponentModel.DataAnnotations;

namespace Users.Application.DTOs
{
	public class UserDto
	{
		public string? Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

	}
}
