using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ABAC.Models
{
    public class User : IdentityUser<int>
    {
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;
        public bool sysAdmin {  get; set; } = false;

        // Navigation property
        public ICollection<LNK_USER_ROLE> UserRoles { get; set; }
    }
}
