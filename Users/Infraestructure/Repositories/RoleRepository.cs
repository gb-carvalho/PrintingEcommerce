using Microsoft.AspNetCore.Identity;
using System.Data;
using Users.Domain.Interfaces;

namespace Users.Infraestructure.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private RoleManager<IdentityRole> _roleManager;

		public RoleRepository(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task<bool> RoleExistsAsync(string role) 
		{
			return await _roleManager.RoleExistsAsync(role);
		}

		public async Task CreateAsync(IdentityRole role)
		{
			await _roleManager.CreateAsync(role);
		}
	}
}
