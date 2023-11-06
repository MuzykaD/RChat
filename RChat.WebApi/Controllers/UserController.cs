using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Users;
using RChat.Domain.Repsonses;
namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsersInformation([FromQuery] int skip, int take, string? value, string? orderBy, string? orderByType)
                => Ok(await _userService.GetUsersInformationListAsync(value, skip, take, orderBy, orderByType));

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsersInformation([FromQuery]string value)
               => Ok(await _userService.SearchUsersInformationListAsync(value));

    }
}
