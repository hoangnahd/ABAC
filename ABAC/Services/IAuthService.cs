using System.Threading.Tasks;
using ABAC.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace ABAC.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(ABAC.Models.LoginRequest model);
        Task<bool> EditUserAsync(string username, EditUserRequest editUserRequest);
        Task<(bool Success, string ErrorMessage)> AddUserAsync(UserRequest userRequest);
        Task<bool> AddRoleAsync(RoleRequest roleRequest);
        Task<bool> LinkRoleToResourceAsync(int roleId, int resourceId);
        Task<bool> LinkUserToRoleAsync(int userId, int roleId);
    }
}
