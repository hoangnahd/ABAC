using ABAC.Services;
using Microsoft.AspNetCore.Mvc;
using ABAC.Data;
using ABAC.Models;
using System.Linq;

namespace ABAC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("access")]
        public IActionResult AccessRequest(string username, string action, string resourceName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            var resource = _context.Resources.FirstOrDefault(r => r.Name == resourceName);

            if (user == null || resource == null)
            {
                return NotFound();
            }

            var decision = _pdp.Evaluate(user, action, resource, _environment);
            if (decision == "allow")
            {
                return Ok("Access granted");
            }
            return Forbid("Access denied");
        }
    }
}
