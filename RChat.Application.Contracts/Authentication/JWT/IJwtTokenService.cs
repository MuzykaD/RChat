using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Authentication.JWT
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtTokenAsync(int userId, string userName, string userEmail);
    }
}
