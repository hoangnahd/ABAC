using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ABAC.Models
{
    public class Roles : IdentityRole<int>
    {

        // Navigation property
        public ICollection<LNK_USER_ROLE> UserRoles { get; set; }
        public ICollection<LNK_ROLE_RESOURCES> RoleResources { get; set; }
    }

}
