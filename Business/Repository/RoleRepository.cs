using Business.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Deletes a role from the database.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>True if the role was deleted, false otherwise.</returns>
        public async Task<bool> DeleteRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return true;

            return false;
        }

        /// <summary>
        /// Gets all roles from the role manager.
        /// </summary>
        /// <returns>A list of IdentityRole objects.</returns>
        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single role by its Id.
        /// </summary>
        /// <param name="roleId">The Id of the role to retrieve.</param>
        /// <returns>A Task containing the retrieved role.</returns>
        public Task<IdentityRole> GetSingleRoleAsync(string roleId)
        {
            return _roleManager.FindByIdAsync(roleId);
        }

        /// <summary>
        /// Updates a role in the database.
        /// </summary>
        /// <param name="role">The role to be updated.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return true;

            return false;
        }

        /// <summary>
        /// Creates a new role in the identity system.
        /// </summary>
        /// <param name="newRole">The new role to be created.</param>
        /// <returns>A boolean indicating whether the role was successfully created.</returns>
        public async Task<bool> CreateNewRole(IdentityRole newRole)
        {
            if (newRole == null)
                return false;

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
                return true;

            return false;
        }

        /// <summary>
        /// Retrieves a role by its name.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The role with the specified name.</returns>
        public Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            return _roleManager.FindByNameAsync(roleName);
        }
    }
}
