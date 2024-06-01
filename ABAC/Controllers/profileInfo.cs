using ABAC.Services;
using Microsoft.AspNetCore.Mvc;
using ABAC.Data;
using ABAC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class profileInfo : ControllerBase
    {
        private readonly PolicyDecisionPoint _pdp;
        private readonly ABAC.Models.Environment _environment;
        private readonly ApplicationDbContext _context;

        public profileInfo(PolicyDecisionPoint pdp, ApplicationDbContext context)
        {
            _pdp = pdp;
            _context = context;
            _environment = new ABAC.Models.Environment { Location = "office", Time = "work_hours" }; // Example environment attributes
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult AccessRequest()
        {
            if (User != null && User.Identity != null)
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
    }
}
