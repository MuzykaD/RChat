using Microsoft.AspNetCore.Identity;
using RChat.Application.Contracts.Authentication;
using RChat.Application.Contracts.Authentication.JWT;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IJwtTokenService _jwtTokenService;
        private UserManager<User> _userManager;

        public AuthenticationService(IJwtTokenService jwtTokenService, UserManager<User> userManager)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }
        public async Task<string?> GetTokenByCredentials(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return await _jwtTokenService.GenerateJwtTokenAsync(user.Id, user.UserName, user.Email);
            }
            else
                return null;
        }

        public Task<string> LogoutUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var userByEmail = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (userByEmail == null)
            {
                var createdUser = new User()
                {
                    Email = registerUserDto.Email,
                    PhoneNumber = registerUserDto.PhoneNumber,
                    UserName = registerUserDto.Username,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    PasswordHash = registerUserDto.Password
                };
                var result = await _userManager.CreateAsync(createdUser, createdUser.PasswordHash);
                return result.Succeeded;
            }
            else
                return false;
        }
    }
}
