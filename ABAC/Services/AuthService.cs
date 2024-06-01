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

        public AuthService(ApplicationDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> LoginAsync(ABAC.Models.LoginRequest model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                return new AuthenticationResult { Success = true, Token = token };
            }

            return new AuthenticationResult { Success = false, Message = "Invalid username or password" };
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
