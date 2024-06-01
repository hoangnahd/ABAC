using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ABAC.Services;
using Microsoft.AspNetCore.Identity.Data;
using ABAC.Models;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(ABAC.Models.LoginRequest loginRequest)
        {
            var token = await authService.LoginAsync(loginRequest);
            if(token != null)
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

    }
}
