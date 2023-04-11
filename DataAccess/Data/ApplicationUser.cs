using Microsoft.AspNetCore.Identity;

namespace DataAccess.Data
{
    // 124. создадим класс ApplicationUser в проекте DataAccess
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
