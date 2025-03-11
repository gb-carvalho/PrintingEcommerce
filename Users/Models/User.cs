using Microsoft.AspNetCore.Identity;

namespace Users.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
    }
}
