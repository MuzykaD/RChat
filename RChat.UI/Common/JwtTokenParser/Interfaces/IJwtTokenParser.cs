using System.Security.Claims;

namespace RChat.UI.Common.JwtTokenParser.Interfaces
{
    public interface IJwtTokenParser
    {
        IEnumerable<Claim> ParseJwtToClaims(string jwtToken);
        bool IsTokenValid(string jwtToken);
    }
}
