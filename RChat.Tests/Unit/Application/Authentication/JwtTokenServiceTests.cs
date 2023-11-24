using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RChat.Application.Authentication.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RChat.Tests.Unit.Application.Authentication
{
    public class JwtTokenServiceTests
    {
        [Fact]
        public async Task GenerateJwtTokenAsync_Should_Generate_Valid_JwtToken()
        {
            // Arrange
            string issuer = "issuer";
            string audience = "audience";
            string key = "VerySecretDummyKeyForMyVerySecretTest";
            string expireTime = "30";

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Jwt:Key"]).Returns(key);
            configurationMock.Setup(x => x["Jwt:Issuer"]).Returns(issuer);
            configurationMock.Setup(x => x["Jwt:Audience"]).Returns(audience);
            configurationMock.Setup(x => x["Jwt:ExpireTime"]).Returns(expireTime);

            var jwtTokenService = new JwtTokenService(configurationMock.Object);

            var userId = 1;
            var userName = "testuser";
            var userEmail = "testuser@example.com";

            // Act
            var token = await jwtTokenService.GenerateJwtTokenAsync(userId, userName, userEmail);

            // Assert
            Assert.NotNull(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No clock skew for simplicity in the test
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            Assert.IsType<JwtSecurityToken>(validatedToken);

            var jwtSecurityToken = (JwtSecurityToken)validatedToken;

            Assert.Equal(userId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(userName, principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.Equal(userEmail, principal.FindFirst(ClaimTypes.Email)?.Value);

            Assert.Equal(issuer, jwtSecurityToken.Issuer);
            Assert.Equal(audience, jwtSecurityToken.Audiences.Single());
        }
    }
}
