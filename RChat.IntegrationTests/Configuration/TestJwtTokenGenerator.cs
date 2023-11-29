using Microsoft.IdentityModel.Tokens;
using RChat.Application.Contracts.Authentication.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Configuration
{
    internal class TestJwtTokenGenerator : IJwtTokenService
    {
        public Task<string> GenerateJwtTokenAsync(int userId, string userName, string userEmail)
        {
            string issuer = "issuer";
            string audience = "audience";
            string expireTime = "30";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VerySecretDummyKeyForMyVerySecretTest"));

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId.ToString()),
                new (ClaimTypes.Name, userName),
                new (ClaimTypes.Email, userEmail),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(expireTime)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }
    }
}
