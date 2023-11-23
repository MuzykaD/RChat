using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Authentication;
using RChat.Application.Contracts.Authentication.JWT;
using RChat.Application.Contracts.Account;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using RChat.WebApi.Hubs;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/account")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountService _userService;
        private IJwtTokenService _jwtService;
        public AccountController(IAccountService userService, IJwtTokenService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangeUserPasswordDto dto)
        {
            string currentUserEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var passwordChangeResult = await _userService.ChangeUserPasswordAsync(currentUserEmail, dto.CurrentPassword, dto.NewPassword);

            return passwordChangeResult ?
                Ok(new ApiResponse()
                {
                    IsSucceed = passwordChangeResult,
                    Message = "Password successfully updated!"
                }) :
                BadRequest(new ApiResponse()
                {
                    IsSucceed = passwordChangeResult,
                    Message = "It seems that you have entered a wrong current password! Try again!"
                });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetPersonalInformationAsync()
        {
            var currentUser = await _userService.GetPersonalInformationAsync(User.FindFirstValue(ClaimTypes.Email)!);

            return Ok(currentUser);               
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateUserDto updateDto)
        {
            var result = await _userService.UpdateUserAsync(User.FindFirstValue(ClaimTypes.Email)!, updateDto);
            return result ?
                Ok(new UserTokenResponse()
                {
                    IsSucceed = result,
                    Token = await _jwtService
                    .GenerateJwtTokenAsync(
                        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                        updateDto.UserName,
                        updateDto.Email)
                }): 
                BadRequest(new UserTokenResponse()
                {
                IsSucceed = result,
                Message = "It seems that provided email is already taken! Try another one!"
                });
        }
    }
}
