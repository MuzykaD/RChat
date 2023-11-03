using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RChat.WebApi.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
        {
            
            serviceCollection
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = jwtSettings.Issuer,
                       ValidateAudience = true,
                       ValidAudience = jwtSettings.Audience,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                   };
               });
        }
    }
}
