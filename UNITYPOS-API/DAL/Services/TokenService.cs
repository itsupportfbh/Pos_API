using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Data.ORM
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public (string Token, DateTime Expiration) GenerateToken(UserMaster user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var issuer = _jwtSettings.Issuer ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var audience = _jwtSettings.Audience ?? throw new InvalidOperationException("JWT Audience is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var userId = user.Id?.ToString() ?? "0";
            var isAdmin = user.IsAdmin?.ToString() ?? bool.FalseString;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("UserId", userId),
                new Claim("Code", user.Code ?? string.Empty),
                new Claim("EmpCode", user.EmpCode ?? string.Empty),
                new Claim("IsAdmin", isAdmin)
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return (token, expiration);
        }
    }
}
