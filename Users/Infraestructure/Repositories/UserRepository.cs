using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infraestructure.Data;


namespace Users.Infraestructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User?> GetByIdAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}
		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}
		public async Task<IdentityResult> CreateAsync(User user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}
		public async Task<IdentityResult> DeleteAsync(string id)
		{
			User? user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				return await _userManager.DeleteAsync(user);
			}
			return IdentityResult.Failed(new IdentityError { Description = "User not found" });
		}

		public async Task<bool> VerifyPasswordAsync(User user, string password)
		{
			return await _userManager.CheckPasswordAsync(user, password);
		}
	}
}
