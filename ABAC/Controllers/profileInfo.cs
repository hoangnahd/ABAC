using ABAC.Services;
using Microsoft.AspNetCore.Mvc;
using ABAC.Data;
using ABAC.Models;
using ABAC.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class profileInfo : ControllerBase
    {
        private readonly PolicyDecisionPoint _pdp;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService authService;
        public profileInfo(PolicyDecisionPoint pdp, ApplicationDbContext context, IAuthService _authService)
        {
            _pdp = pdp;
            _context = context;
            authService = _authService;
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult AccessRequest()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (user == null)
                {
                    return NotFound("User not found!!");
                }
                var userResponse = new userReponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Department = user.Department,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return Ok(userResponse);
            }
            return NotFound("Unathorized");
        }
        [HttpPut("profile-edit")]
        [Authorize]
        public async Task<IActionResult> EditUser(EditUserRequest editUserRequest)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var result = await authService.EditUserAsync(User.Identity.Name, editUserRequest);
                if (result)
                {
                    return Ok("User information updated successfully.");
                }
                return NotFound("User not found.");
            }
            return NotFound("Unathorized");
        }
    }
}
