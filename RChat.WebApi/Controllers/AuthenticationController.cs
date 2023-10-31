using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Authentication;
using RChat.Domain.Messages;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System.Runtime.CompilerServices;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/authentication")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginUser)
        {
            var token = await _authenticationService
                .GetTokenByCredentials(loginUser.Email, loginUser.Password);

            return string.IsNullOrWhiteSpace(token) ?
                Unauthorized(new UserTokenResponse()
                {
                    IsSucceed = false,
                    Message = "Wrong credentials",
                }) :
                Ok(new UserTokenResponse()
                {
                    IsSucceed = true,
                    Token = token,
                });
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await _authenticationService.RegisterUserAsync(registerUserDto);
            return result ? 
                Ok(new ApiResponse()
                { 
                    IsSucceed = true, 
                    Message = "Account created! You can use credentials to log in!" 
                }) : 
                BadRequest(new ApiResponse()
                {
                    IsSucceed = true,
                    Message = "Account with such email already exists, try another one!"
                });
            
        }

    }
}
