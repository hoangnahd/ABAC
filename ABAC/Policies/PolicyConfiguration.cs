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
                                .Where(rr => rr.RoleId == role.Id && resource != null && rr.ResourceId == resource.Id)
                                .ToList();

                            if (roleResources.Any())
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                },
                new Policy
                {
                    Action = "write",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return resource != null && resource.Owner.Contains(user.Id);
                    }
                },
                new Policy
                {
                    Action = "read",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return resource != null && resource.Owner.Contains(user.Id);
                    }
                },
                new Policy
                {
                    Action = "read",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return resource != null && user.Department == resource.Department &&  resource.Sensitivity == "Medium";
                    }
                },
                new Policy
                {
                    Action = "read",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return  resource != null &&  resource.Sensitivity == "Low";
                    }
                },
                new Policy
                {
                    Action = "read",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return user.sysAdmin;
                    }
                },
                new Policy
                {
                    Action = "create-role",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return user.sysAdmin;
                    }
                },
                new Policy
                {
                    Action = "create-user",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return user.sysAdmin;
                    }
                },
                new Policy
                {
                    Action = "lnk-role-resource",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return user.sysAdmin;
                    }
                },
                new Policy
                {
                    Action = "lnk-user-role",
                    ResourceType = "Document",
                    Conditions = (user, resource, context) =>
                    {
                        return user.sysAdmin;
                    }
                }

            };
        }
    }
}
