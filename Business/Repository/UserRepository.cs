using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Deletes a user from the identity database.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>True if the user was successfully deleted, false otherwise.</returns>
        public async Task<bool> DeleteUserAsync(IdentityUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return true;
            return false;
        }

        /// <summary>
        /// Gets a list of all users from the UserManager.
        /// </summary>
        /// <returns>
        /// An IEnumerable of IdentityUser objects.
        /// </returns>
        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList;
        }

        /// <summary>
        /// Retrieves a single user from the user manager based on the userId.
        /// </summary>
        /// <param name="userId">The userId of the user to retrieve.</param>
        /// <returns>The user with the specified userId.</returns>
        public async Task<IdentityUser> GetSingleUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>True if the user was successfully updated, false otherwise.</returns>
        public async Task<bool> UpdateUserAsync(IdentityUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the current user role asynchronously.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns>The current user role.</returns>
        public async Task<IdentityRole?> GetCurrentUserRoleAsync(IdentityUser currentUser)
        {
            var currentRoleNames = await _userManager.GetRolesAsync(currentUser);
            var name = currentRoleNames.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(name))
                return await _roleManager.FindByNameAsync(name);

            return null;
        }

        /// <summary>
        /// Updates the role of a given user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="newRole">The new role to assign to the user.</param>
        /// <returns>True if the role was successfully updated, false otherwise.</returns>
        public async Task<bool> UpdateUserRole(IdentityUser user, IdentityRole newRole)
        {
            if (!string.IsNullOrWhiteSpace(newRole.Id))
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any())
                    _ = await _userManager.RemoveFromRolesAsync(user, roles);

                var result = await _userManager.AddToRoleAsync(user, newRole.Name);

                if (result.Succeeded)
                    return true;
                return false;
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (result.Succeeded)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets all users with their associated roles.
        /// </summary>
        /// <returns>A list of users and their associated roles.</returns>
        public async Task<IEnumerable<UserAndRole>> GetAllUsersWithRoleAsync()
        {
            List<UserAndRole> users = new();

            var tempUsers = await _userManager.Users.ToListAsync();

            foreach (var item in tempUsers)
            {
                users.Add(new UserAndRole
                {
                    User = item,
                    Role = await GetCurrentUserRoleAsync(item)
                });
            }

            return users;
        }
    }
}
