using Users.Application.DTOs;
using Users.Domain.Interfaces;
using Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Users.Application.Services
{
	public class UserService
	{
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userRepository;

		public UserService(IConfiguration configuration, IUserRepository userRepository)
		{
			_configuration = configuration;
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
		{
			IEnumerable<User> users = await _userRepository.GetAllAsync();
			return users.Select(u => new UserDto { Id = u.Id, Name = u.Name, Email = u.Email ?? "" });
		}
		public async Task<UserDto?> GetUserByIdAsync(string id)
		{
			User? user = await _userRepository.GetByIdAsync(id);
			return user == null ? null : new UserDto { Id = user.Id, Name = user.Name, Email = user.Email ?? "" };
		}
		public async Task<UserDto?> GetUserByEmailAsync(string email)
		{
			User? user = await _userRepository.GetByEmailAsync(email);
			return user == null ? null : new UserDto { Id = user.Id, Name = user.Name, Email = user.Email ?? "" };
		}

		public async Task<IdentityResult> CreateUserAsync(UserDto user, string password)
		{
			User newUser = new User { Name = user.Name, Email = user.Email ?? "", UserName = user.Email };
			return await _userRepository.CreateAsync(newUser, password);
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
}
