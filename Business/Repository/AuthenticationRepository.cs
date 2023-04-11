using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;

namespace Business.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationRepository(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Logs in a user with the given username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="rememberMe">Whether to remember the user.</param>
        /// <returns>True if the user was successfully logged in, false otherwise.</returns>
        public async Task<bool> Login(string username, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Logs out the current user from the application.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously registers a new user.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<bool> RegisterAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}