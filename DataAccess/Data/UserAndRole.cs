using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class UserAndRole
    {
        public IdentityUser User { get; set; }
        public IdentityRole Role { get; set; }
    }
}
