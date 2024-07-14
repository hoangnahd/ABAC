using System.Data;
using Microsoft.AspNetCore.Identity;

namespace ABAC.Models
{
    public class LNK_USER_ROLE
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Roles Role { get; set; }
    }
}
