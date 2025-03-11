using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Users.Models;
using System.ComponentModel.DataAnnotations;

namespace Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userManager.Users.ToListAsync();
			return Ok(users);

		}
		
		[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
				
			var existingUser = await _userManager.FindByEmailAsync(user.Email);
			if (existingUser != null)
				return BadRequest("Usuário já existe com este e-mail.");

			var createdUser = new User { Name = user.Name, Email = user.Email, UserName = user.Email };

            var result = await _userManager.CreateAsync(createdUser, user.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok("Usuário registrado com sucesso!");

		}
	}

	public class RegisterUserDto
	{
		[Required]
		public required string Name { get; set; }

		[Required]
		[EmailAddress]
		public required string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public required string Password { get; set; }
	}
}
