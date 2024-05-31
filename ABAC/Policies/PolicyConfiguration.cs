namespace ABAC.Policies
{
    public static class PolicyConfiguration
    {
        public static List<Policy> GetPolicies()
        {
            return new List<Policy>
            {
                new Policy
                {
                    Action = "read",
                    ResourceType = "report",
                    Conditions = (user, resource, env) =>
                        user.Role == "manager" && user.Department == "sales" && env.Time == "work_hours"
                },
                // Add more policies as needed
            };
        }
    }
}
