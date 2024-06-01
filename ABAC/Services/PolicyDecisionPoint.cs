using ABAC.Data;
using ABAC.Models;
using ABAC.Policies;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ABAC.Services
{
    public class PolicyDecisionPoint
    {
        private readonly ApplicationDbContext _context;
        private readonly List<Policy> _policies;

        public PolicyDecisionPoint(ApplicationDbContext context, List<Policy> policies)
        {
            _context = context;
            _policies = policies;
        }

        public bool Evaluate(User user, string action, Resource resource)
        {
            // Check if the user has direct permissions for the resource and action
            if(user.sysAdmin || 
                resource.Sensitivity == "Low" || 
                (resource.Sensitivity == "Medium" && 
                user.Department == resource.Department)
                )
                return true;

            var policyBasedPermission = _policies
                .Any(policy => policy.Action == action &&
                               policy.Conditions(user, resource, _context));

            return policyBasedPermission;
        }
    }
}
