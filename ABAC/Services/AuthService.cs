using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ABAC.Models;
using ABAC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel;

namespace ABAC.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;

        public AuthService(ApplicationDbContext context, JwtSettings jwtSettings, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> LoginAsync(ABAC.Models.LoginRequest model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                return new AuthenticationResult { Success = true, TokenValue = token };
            }

            return new AuthenticationResult { Success = false, Message = "Invalid username or password" };
        }
        public async Task<(bool Success, string ErrorMessage)> AddUserAsync(UserRequest userRequest)
        {
            // Check if a user with the given username already exists
            var existingUser = await _userManager.FindByNameAsync(userRequest.UserName);
            if (existingUser != null)
            {
                // User already exists, return false or handle as needed
                return (false, "User already exists.");
            }

            var newUser = new User
            {
                UserName = userRequest.UserName,
                Department = userRequest.Department,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true, // Assuming you want to confirm the phone number
                LockoutEnabled = false, // Assuming you don't want to enable lockout
                AccessFailedCount = 0,
            };

            var result = await _userManager.CreateAsync(newUser, userRequest.Password);
            if (!result.Succeeded)
            {
                // Collect error messages
                string errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<bool> LinkUserToRoleAsync(int userId, int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            var link = new LNK_USER_ROLE
            {
                RoleId = roleId,
                UserId = userId
            };
            if (role == null)
            {
                return false;
            }

            _context.UserRole.Add(link);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddRoleAsync(RoleRequest roleRequest)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleRequest.RoleName);
            if (roleExists)
            {
                return false;
            }

            var role = new IdentityRole<int> { Name = roleRequest.RoleName };
            var result = await _roleManager.CreateAsync(role);
            await _context.SaveChangesAsync();
            return result.Succeeded;
        }
        public async Task<bool> LinkRoleToResourceAsync(int roleId, int resourceId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            var resource = await _context.Resources.FindAsync(resourceId);

            if (role == null || resource == null)
            {
                return false;
            }

            var link = new LNK_ROLE_RESOURCES
            {
                RoleId = roleId,
                ResourceId = resourceId
            };

            _context.RoleResources.Add(link);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditUserAsync(string username, EditUserRequest editUserRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return false;
            }

            user.FirstName = editUserRequest.FirstName;
            user.LastName = editUserRequest.LastName;
            user.Department = editUserRequest.Department;
            user.Email = editUserRequest.Email;
            user.PhoneNumber = editUserRequest.PhoneNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(null, storedPasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
