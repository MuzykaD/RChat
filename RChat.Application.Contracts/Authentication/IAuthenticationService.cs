using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Authentication
{
    public interface IAuthenticationService
    {
        Task<string?> GetTokenByCredentials(string email, string password);
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<string> LogoutUserAsync();
    }
}
