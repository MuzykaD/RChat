using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RChat.Application.Contracts.Authentication.JWT;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;


namespace RChat.Application.Authentication.JWT
{
    public class JwtTokenService : IJwtTokenService
    {
        IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> GenerateJwtTokenAsync(int userId, string userName, string userEmail)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var claims = new List<Claim>
            { 
                new (ClaimTypes.NameIdentifier, userId.ToString()),
                new (ClaimTypes.Name, userName),
                new (ClaimTypes.Email, userEmail),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireTime"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }
    }
}
