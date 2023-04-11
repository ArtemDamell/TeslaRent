using Microsoft.AspNetCore.Identity;

namespace DataAccess.Data
{
    public class UserAndRole
    {
        public IdentityUser User { get; set; }
        public IdentityRole Role { get; set; }
    }
}
