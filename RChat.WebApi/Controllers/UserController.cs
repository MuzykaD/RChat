using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Users;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using System.Security.Claims;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangeUserPasswordDto dto)
        {
            string currentUserEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var passwordChangeResult = await _userService.ChangeUserPasswordAsync(currentUserEmail, dto.CurrentPassword, dto.NewPassword);

            return passwordChangeResult ?
                Ok(new ApiResponse()
                {
                    IsSucceed = passwordChangeResult,
                    Message = "Password successfully update!"
                }) :
                BadRequest(new ApiResponse()
                {
                    IsSucceed = passwordChangeResult,
                    Message = "It seems that you have entered a wrong current password! Try again"
                });
        }
    }
}
