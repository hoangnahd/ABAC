using ABAC.Models;
using ABAC.Data;
public class Policy
{
    public int PolicyId { get; set; } // Add an identifier for each policy
    public string Name { get; set; } = string.Empty; // Add a name to identify the policy
    public string Action { get; set; } = string.Empty; // Action the policy applies to (e.g., "read", "write")
    public string ResourceType { get; set; } = string.Empty; // Type of resource the policy applies to (e.g., "file", "document")
    public Func<User, Resource, ApplicationDbContext, bool> Conditions { get; set; } // Function to evaluate conditions
}
