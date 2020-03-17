using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string UserGroupName { get; set; }

        public int UserGroupId { get; set; }
    }
}
