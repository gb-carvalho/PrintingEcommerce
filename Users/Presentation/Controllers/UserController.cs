using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Users.Application.Services;
using Users.Application.DTOs;


namespace Users.Presentation.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly UserService _userService;

		public UserController(IConfiguration configuration, UserService userService)
		{
			_configuration = configuration;
			_userService = userService;
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserDto user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);	

			UserDto? existingUser = await _userService.GetUserByEmailAsync(user.Email);
			if (existingUser != null)
				return BadRequest("Usuário já existe com este e-mail.");

			UserDto createdUser = new UserDto { Name = user.Name, Email = user.Email};

			IdentityResult? result = await _userService.CreateUserAsync(createdUser, user.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok("Usuário registrado com sucesso!");

		}

		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			UserDto? user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound("Usuário não encontrado.");
			}

			IdentityResult? result = await _userService.DeleteUserAsync(id);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return Ok("Usuário deletado com sucesso!");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			UserDto? user = await _userService.GetUserByEmailAsync(request.Email);
			if (user == null || !await _userService.VerifyUserPasswordAsync(user, request.Password))
				return Unauthorized("Email ou senha inválidos.");

			var token = _userService.GenerateJwtToken(user);
			return Ok(new { Token = token });
		}
				
	}

	public class LoginRequest
	{
		public required string Email { get; set; }
		public required string Password { get; set; }
	}
}
