using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ABAC.Services;
using Microsoft.AspNetCore.Identity.Data;
using ABAC.Models;
using ABAC.Data;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ApplicationDbContext _context;
        public AuthController(IAuthService _authService, ApplicationDbContext context)
        {
            authService = _authService;
            _context = context;
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
