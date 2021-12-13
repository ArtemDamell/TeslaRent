using Business.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> DeleteRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetSingleRoleAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<bool> UpdateRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<bool> CreateNewRole(IdentityRole newRole)
        {
            if (newRole == null)
                return false;

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
                return true;

            return false;
        }
    }
}
