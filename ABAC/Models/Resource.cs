using System.ComponentModel.DataAnnotations;

namespace ABAC.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Sensitivity { get; set; }
        public string Department { get; set; }
        public string Content { get; set; }
        
        // Navigation property
        public ICollection<LNK_ROLE_RESOURCES> RoleResources { get; set; }
    }

}
