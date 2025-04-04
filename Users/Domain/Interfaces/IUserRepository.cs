using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllAsync();
		Task<User?> GetByIdAsync(string id);
		Task<User?> GetByEmailAsync(string email);
		Task<IdentityResult> CreateAsync(User user, string password);
		Task<IdentityResult> DeleteAsync(string id);
		Task<bool> VerifyPasswordAsync(User user, string password);
		Task<IList<string>> GetAllRolesAsync(User user);
		Task RemoveRolesAsync(User user, IEnumerable<string> roles);
		Task<IdentityResult> AddRolesAsync(User user, string role);
	}
}
