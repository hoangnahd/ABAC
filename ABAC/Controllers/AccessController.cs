using ABAC.Services;
using Microsoft.AspNetCore.Mvc;
using ABAC.Data;
using ABAC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/access")]
    public class AccessController : ControllerBase
    {
        private readonly PolicyDecisionPoint _pdp;
        private readonly ABAC.Models.Environment _environment;
        private readonly ApplicationDbContext _context;

        public AccessController(PolicyDecisionPoint pdp, ApplicationDbContext context)
        {
            _pdp = pdp;
            _context = context;
            _environment = new ABAC.Models.Environment { Location = "office", Time = "work_hours" }; // Example environment attributes
        }

        [HttpGet("resource")]
        [Authorize]
        public IActionResult AccessRequest(string action, int resourceId)
        {
            if(User != null && User.Identity != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (user == null)
                {
                    return NotFound("User not found!!");
                }

                var resource = _context.Resources.FirstOrDefault(r => r.Id == resourceId);
                if (resource == null)
                {
                    return NotFound("Resource not found!!");
                }
                
                var decision = _pdp.Evaluate(user, action, resource);
                
                if (!decision)
                {
                    return Forbid("Access denied");
                }
                return Ok(resource.Content);
            }
            return NotFound("User not found!");  
        }
        [HttpGet("AccessUserInfo")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult getAccessUserInfo(int userId)
        {
            if (User != null && User.Identity != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                
                if (user == null || !user.sysAdmin)
                {
                    return NotFound("Unauthorized");
                }

                var getUser = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (getUser == null)
                {
                    return NotFound("User not found!!");
                }

                var userResponse = new userReponse
                {
                    Id = user.Id,
                    UserName = getUser.UserName,
                    Department = getUser.Department,
                    Email = getUser.Email,
                    PhoneNumber = getUser.PhoneNumber
                };

                return Ok(userResponse);
            }
            return NotFound("User not found!");
        }
        [HttpGet("AccessAllUserInfo")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            if(User != null && User.Identity != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user == null || !user.sysAdmin)
                {
                    return NotFound("Unauthorized");
                }
                var users = _context.Users.Select(user => new userReponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Department = user.Department,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }).ToList();
                if(users == null)
                {
                    NotFound("User not found!");
                }

                return Ok(users);
            }
            return NotFound("User not found!");

        }
    }
}
