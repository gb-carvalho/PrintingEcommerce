using Users.Application.DTOs;
using Users.Domain.Interfaces;
using Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;

namespace Users.Application.Services
{
	public class UserService
	{
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userRepository;
		private readonly RoleManager<IdentityRole> _roleManager;


		public UserService(IConfiguration configuration, IUserRepository userRepository, 
							RoleManager<IdentityRole> roleManager)
		{
			_configuration = configuration;
			_userRepository = userRepository;
			_roleManager = roleManager;
		}

		public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
		{
			IEnumerable<User> users = await _userRepository.GetAllAsync();

			List<UserDto> userDtos = new List<UserDto>();

			foreach(User user in users)
			{
				var roles = await _userRepository.GetAllRolesAsync(user);

				UserDto userDto = new UserDto
				{
					Id = user.Id,
					Name = user.Name,
					Email = user.Email ?? "",
					Role = roles.FirstOrDefault() ?? ""
				};

				userDtos.Add(userDto);
			}

			return userDtos;
		}
		public async Task<UserDto?> GetUserByIdAsync(string id)
		{
			User? user = await _userRepository.GetByIdAsync(id);
			if (user == null) 
				return null;
			var roles = await _userRepository.GetAllRolesAsync(user);
			return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email ?? "", Role = roles.FirstOrDefault()};
		}
		public async Task<UserDto?> GetUserByEmailAsync(string email)
		{
			User? user = await _userRepository.GetByEmailAsync(email);
			if (user == null)
				return null;
			var roles = await _userRepository.GetAllRolesAsync(user);
			return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email ?? "", Role = roles.FirstOrDefault()	};
		}

		public async Task<IdentityResult> CreateUserAsync(UserDto user, string password, string role = "User")
		{
			User newUser = new User { Name = user.Name, Email = user.Email ?? "", UserName = user.Email };
			IdentityResult result = await _userRepository.CreateAsync(newUser, password);
			if (result.Succeeded)
			{
				return await AssignRoleToUserAsync(newUser.Id, role);
			}
			return result;
		}

		public async Task<IdentityResult> DeleteUserAsync(string id)
		{
			return await _userRepository.DeleteAsync(id);
		}

		public async Task<bool> VerifyUserPasswordAsync(UserDto userDto, string password)
		{
			User? user = await _userRepository.GetByEmailAsync(userDto.Email);
			if (user == null)
				return false;
			return await _userRepository.VerifyPasswordAsync(user, password);
		}

		public string GenerateJwtToken(UserDto user)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? ""));

			List<Claim> claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? ""),
				new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
				new Claim(ClaimTypes.Name, user.Name),
				new Claim(ClaimTypes.Role, user.Role ?? "")
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

		public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string newRole)
		{
			User? user = await _userRepository.GetByIdAsync(userId);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "Usuário não encontrado." });
			}

			bool roleExists = await _roleManager.RoleExistsAsync(newRole);
			if (!roleExists)
			{
				return IdentityResult.Failed(new IdentityError { Description = "Role não existe." });
			}

			var currentRoles = await _userRepository.GetAllRolesAsync(user);
			await _userRepository.RemoveRolesAsync(user, currentRoles);

			return await _userRepository.AddRolesAsync(user, newRole);
		}
		public async Task EnsureRolesCreated()
		{
			string[] roles = new[] { "Admin", "User" };
			foreach (string role in roles)
			{
				if (!await _roleManager.RoleExistsAsync(role))
				{
					await _roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
		public async Task EnsureAdminCreated()
		{
			string adminEmail = "admin@example.com";
			string adminPassword = "Admin123@";

			if ( await _roleManager.RoleExistsAsync("admin")) 
			{
				await _roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			User? adminUser = await _userRepository.GetByEmailAsync(adminEmail);
			if(adminUser == null)
			{
				adminUser = new User { UserName = adminEmail, Email = adminEmail, Name = "Admin"};
				IdentityResult result = await _userRepository.CreateAsync(adminUser, adminPassword);
				if (result.Succeeded)
				{
					await _userRepository.AddRolesAsync(adminUser, "Admin");
				}
			}
			else
			{
				var roles = await _userRepository.GetAllRolesAsync(adminUser);
				if (!roles.Contains("Admin"))
				{
					await _userRepository.AddRolesAsync(adminUser, "Admin");
				}
			}
		}
	}	
}
