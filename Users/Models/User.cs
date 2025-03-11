using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class User : IdentityUser
    {
		[Required]
		public required string Name { get; set; }
	}
}
