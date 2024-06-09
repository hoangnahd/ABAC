using ABAC.Services;
using Microsoft.AspNetCore.Mvc;
using ABAC.Data;
using ABAC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/access")]
    public class AccessController : ControllerBase
    {
        private readonly PolicyDecisionPoint _pdp;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService authService;
        public AccessController(PolicyDecisionPoint pdp, ApplicationDbContext context, IAuthService _authService)
        {
            _pdp = pdp;
            _context = context;
            authService = _authService;
        }

        [HttpGet("resource/{id}")]
        [Authorize]
        public IActionResult GetResource(int id, string action)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (user == null)
                {
                    return NotFound("User not found!!");
                }

                var resource = _context.Resources.FirstOrDefault(r => r.Id == id);
                if (resource == null)
                {
                    return NotFound("Resource not found!!");
                }

                var decision = _pdp.Evaluate(user, action, resource);

                if (!decision)
                {
                    return Forbid("Access denied");
                }

                if (action.ToLower() == "read")
                {
                    return Ok(resource.Content);
                }
                else
                {
                    return BadRequest("Invalid action specified for GET.");
                }
            }
            return NotFound("User not found!");
        }


        [HttpGet("AccessUserInfo")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult getAccessUserInfo(int userId)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
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
            if(User != null && User.Identity != null && User.Identity.IsAuthenticated)
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
        [HttpPost("add-role")]
        [Authorize]
        public async Task<IActionResult> AddRole(RoleRequest roleRequest)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {

                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user == null)
                {
                    return NotFound("User not found!!");
                }
                var decision = _pdp.Evaluate(user, "create-role", null);
                if (!decision)
                {
                    return Forbid("Access denied. Not a system administrator.");
                }

                var result = await authService.AddRoleAsync(roleRequest);
                if (result)
                {
                    return Ok("Role created successfully.");
                }
                return BadRequest("Role already exists.");
            }
            return NotFound("User not found!");
        }
        [HttpPost("link-role-to-resource")]
        [Authorize]
        public async Task<IActionResult> LinkRoleToResource(int roleId, int resourceId)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user == null)
                {
                    return NotFound("User not found!!");
                }
                var decision = _pdp.Evaluate(user, "lnk-role-resource", null);
                if (!decision)
                {
                    return Forbid("Access denied. Not a system administrator.");
                }
                var result = await authService.LinkRoleToResourceAsync(roleId, resourceId);
                if (result)
                {
                    return Ok("Role linked to resource successfully.");
                }
                return BadRequest("Failed to link role to resource.");
            }
            return NotFound("User not found!");
        }
        [HttpPost("link-user-to-role")]
        [Authorize]
        public async Task<IActionResult> LinkUserToRole(int userId, int roleId)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound("User not found!!");
                }
                var decision = _pdp.Evaluate(currentUser, "lnk-user-role", null);
                if (!decision)
                {
                    return Forbid("Access denied. Not a system administrator.");
                }
                var result = await authService.LinkUserToRoleAsync(userId, roleId);
                if (result)
                {
                    return Ok("User linked to Role successfully.");
                }
                return BadRequest("Failed to link role to resource.");
            }
            return NotFound("User not found!");
        }
        [HttpPost("add-user")]
        [Authorize]
        public async Task<IActionResult> AddUser(UserRequest userRequest)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var isUserExsist = _context.Users.FirstOrDefault(u => u.UserName == userRequest.UserName);
                var currentUser = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user == null)
                {
                    return NotFound("User not found!!");
                }
                var decision = _pdp.Evaluate(user, "create-user", null);
                if (!decision)
                {
                    return Forbid("Access denied. Not a system administrator.");
                }
                var (result, errorMessage) = await authService.AddUserAsync(userRequest);
                if (result)
                {
                    return Ok("User created successfully.");
                }
                return BadRequest(errorMessage ?? "Failed to create user.");
            }
            return NotFound("User not found!");
        }
        [HttpPost("isSysAdmin")]
        [Authorize]
        public async Task<IActionResult> IsSysAdmin()
        {
            Console.WriteLine("chekc");
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                if (user.sysAdmin)
                {
                    return Ok("true");
                }
            }

            return Unauthorized();
        }
    }
}
