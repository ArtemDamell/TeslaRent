using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> DeleteUserAsync(IdentityUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList;
        }

        public async Task<IdentityUser> GetSingleUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<bool> UpdateUserAsync(IdentityUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return true;
            else
                return false;
        }

        public async Task<IdentityRole?> GetCurrentUserRoleAsync(IdentityUser currentUser)
        {
            var currentRoleNames = await _userManager.GetRolesAsync(currentUser);
            var name = currentRoleNames.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(name))
                return await _roleManager.FindByNameAsync(name);

            return null;
        }

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
                var result = _userManager.RemoveFromRolesAsync(user, roles);

                if (result.Result.Succeeded)
                    return true;
                return false;
            }
        }

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
