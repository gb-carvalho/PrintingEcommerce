using Microsoft.AspNetCore.Identity;

namespace Users.Domain.Interfaces
{
	public interface IRoleRepository
	{
		Task<bool> RoleExistsAsync(string role);
		Task CreateAsync(IdentityRole role);
	}
}
