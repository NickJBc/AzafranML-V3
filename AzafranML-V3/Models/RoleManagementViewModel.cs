using AzafranML_V3.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AzafranML_V3.Models
{
    public class RoleManagementViewModel
    {

        public IList<IdentityRole>? Roles { get; set; }
        public string? RoleName { get; set; } // For creating a new role
        public IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public Dictionary<string, IList<string>> UserRoles { get; set; } = new Dictionary<string, IList<string>>();
    }
}
