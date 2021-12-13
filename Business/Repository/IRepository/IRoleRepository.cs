using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IRoleRepository
    {
		Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
		Task<IdentityRole> GetSingleRoleAsync(string roleId);
		Task<bool> UpdateRoleAsync(IdentityRole role);
		Task<bool> DeleteRoleAsync(IdentityRole role);
		Task<bool> CreateNewRole(IdentityRole newRole);
	}
}
