using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Users.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace Users.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public UserController(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;

		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer")]
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

			User? existingUser = await _userManager.FindByEmailAsync(user.Email);
			if (existingUser != null)
				return BadRequest("Usuário já existe com este e-mail.");

			User createdUser = new User { Name = user.Name, Email = user.Email, UserName = user.Email };

			IdentityResult? result = await _userManager.CreateAsync(createdUser, user.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok("Usuário registrado com sucesso!");

		}

		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			User? user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("Usuário não encontrado.");
			}

			IdentityResult? result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return Ok("Usuário deletado com sucesso!");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			User? user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
				return Unauthorized("Email ou senha inválidos.");

			var token = GenerateJwtToken(user);
			return Ok(new { Token = token });
		}
		
		private string GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? ""));

			List<Claim> claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
				new Claim(ClaimTypes.Name, user.Name)
			};

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationInMinutes"])),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

	public class LoginRequest
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
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
