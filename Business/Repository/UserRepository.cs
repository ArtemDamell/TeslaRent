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
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
    }
}
