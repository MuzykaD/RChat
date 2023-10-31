using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Authentication;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System.Runtime.CompilerServices;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginUserDto loginUser)
        {
            var token = await _authenticationService
                .GetTokenByCredentials(loginUser.Email, loginUser.Password);

            return string.IsNullOrWhiteSpace(token) ?
                Unauthorized("Provided credentials are not valid for this user!") :
                Ok(token);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterUserDto registerUserDto)
        {
            var result = await _authenticationService.RegisterUserAsync(registerUserDto);
            return result ? 
                Ok("Account created! You can use credentials to log in!") : 
                BadRequest("Account with such email already exists, try another one!");
        }

    }
}
