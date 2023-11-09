using RChat.UI.Common.JwtTokenParser.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Text.Json;

namespace RChat.UI.Common.JwtTokenParser
{
    public class JwtTokenParser : IJwtTokenParser
    {
        public bool IsTokenValid(string jwtToken)
        {
            if(string.IsNullOrWhiteSpace(jwtToken))
                return false;
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(jwtToken);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }

        public IEnumerable<Claim> ParseJwtToClaims(string jwtToken)
        {
            var payload = jwtToken.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs =  JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch(base64.Length % 4) 
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
