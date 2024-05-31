using ABAC.Models;

namespace ABAC.Services
{
    public class PolicyDecisionPoint
    {
        private readonly List<Policy> _policies;

        public PolicyDecisionPoint(List<Policy> policies)
        {
            _policies = policies;
        }

        public string Evaluate(User user, string action, Resource resource, ABAC.Models.Environment environment)
        {
            foreach (var policy in _policies)
            {
                if (policy.Action == action && resource.Type == policy.ResourceType)
                {
                    if (policy.Conditions(user, resource, environment))
                    {
                        return "allow";
                    }
                }
            }
            return "deny";
        }
    }
}
