using System.Text;
using PingPong_Authentication_Domain.Services;
using PingPong_Authentication_Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace PingPong_Authentication_Infrastructure.Services
{
    internal class Jwt(IOptions<JwtSettings> jwtSettings) : IJwt
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));

        public Task<string> Generate(Guid id, string email)
        {
            SigningCredentials signingCredentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);
            List<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("Jti", Guid.NewGuid().ToString())
            ];

            JwtSecurityToken securityToken = new (
                issuer: _jwtSettings.Issuer, 
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Minutes), 
                audience: _jwtSettings.Audience, 
                claims: claims, 
                signingCredentials: signingCredentials
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(securityToken));
        }
    }
}
