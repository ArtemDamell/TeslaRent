using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
	public interface IUserRepository
	{
		Task<IEnumerable<IdentityUser>> GetAllUsersAsync();
		Task<IdentityUser> GetSingleUserAsync(string userId);
		Task<bool> UpdateUserAsync(IdentityUser user);
		Task<bool> DeleteUserAsync(IdentityUser user);
	}
}
