using System.Runtime.CompilerServices;

namespace RChat.WebApi.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtSettings GetJwtSettings(this ConfigurationManager configurationManager)
        {
            return new JwtSettings()
            {
                Issuer = configurationManager.GetValue<string>("Jwt:Issuer")!,
                Audience = configurationManager.GetValue<string>("Jwt:Audience")!,
                Key = configurationManager.GetValue<string>("Jwt:Key")!,
            };
        }
    }

    public record JwtSettings
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string Key { get; init; }
    }
    
}
