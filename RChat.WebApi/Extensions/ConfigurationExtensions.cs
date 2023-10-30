using System.Runtime.CompilerServices;

namespace RChat.WebApi.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtSettings GetJwtSettings(this ConfigurationManager configurationManager)
        {
            return new JwtSettings()
            {
                Issuer = configurationManager.GetValue<string>("JwtSettings:Issuer")!,
                Audience = configurationManager.GetValue<string>("JwtSettings:Audience")!,
                Key = configurationManager.GetValue<string>("JwtSettings:Key")!,
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
