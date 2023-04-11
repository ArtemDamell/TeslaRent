using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IAuthenticationRepository
    {
        Task<bool> Login(string username, string password, bool rememberMe);
        Task<bool> LogoutAsync();
        Task<bool> RegisterAsync(ApplicationUser user);
    }
}
