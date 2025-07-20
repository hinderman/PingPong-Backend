using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PingPong_ApiGateway_Domain.Services;
using PingPong_ApiGateway_Infrastructure.Settings;


namespace PingPong_ApiGateway_Infrastructure.Services
{
    internal class Jwt(IOptions<JwtSettings> jwtSettings) : IJwt
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));

        public Task<string> Generate(Guid id, string email)
        {
            SigningCredentials signingCredentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)), SecurityAlgorithms.HmacSha256);
            List<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("Jti", Guid.NewGuid().ToString())
            ];

            JwtSecurityToken securityToken = new(
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
