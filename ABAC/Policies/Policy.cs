using ABAC.Models;
public class Policy
{
    public string Action { get; set; }
    public string ResourceType { get; set; }
    public Func<User, Resource, ABAC.Models.Environment, bool> Conditions { get; set; }
}
