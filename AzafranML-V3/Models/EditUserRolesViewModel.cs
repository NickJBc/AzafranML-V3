using System.Collections.Generic;

namespace AzafranML_V3.Models
{
    public class EditUserRolesViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }  // Add this line
        public IList<string> UserRoles { get; set; } = new List<string>();
        public IList<string> AllRoles { get; set; } = new List<string>();
        public IList<string> SelectedRoles { get; set; } = new List<string>();
    }
}
