using System.Data;

namespace ABAC.Models
{
    public class LNK_ROLE_RESOURCES
    {
        public int RoleId { get; set; }
        public Roles Role { get; set; }

        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
    }

}

