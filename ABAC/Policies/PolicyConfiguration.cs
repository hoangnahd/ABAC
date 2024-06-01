using System.Collections.Generic;
using ABAC.Models;

namespace ABAC.Policies
{
    public static class PolicyConfiguration
    {
        public static List<Policy> GetPolicies()
        {
            return new List<Policy>
            {
                // Define policies for HR_Manager role
                new Policy
                {
                    Action = "read",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        var userRoles = context.UserRole
                            .Where(ur => ur.UserId == user.Id)
                            .Select(ur => ur.Role)
                            .ToList();

                        foreach (var role in userRoles)
                        {
                            var roleResources = context.RoleResources
                                .Where(rr => rr.RoleId == role.Id && rr.ResourceId == resource.Id)
                                .ToList();

                            if (roleResources.Any())
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                
            };
        }
    }
}
