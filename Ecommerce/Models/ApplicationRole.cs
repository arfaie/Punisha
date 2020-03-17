using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}
