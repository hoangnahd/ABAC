using System.Threading.Tasks;
using ABAC.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace ABAC.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(ABAC.Models.LoginRequest model);
    }
}
