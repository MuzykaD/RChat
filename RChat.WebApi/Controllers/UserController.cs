using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Users;
using RChat.Domain;
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
        public async Task<IActionResult> GetUsersInformation([FromQuery] int page, int size, string? value, string? orderBy, string? orderByType)
                => Ok(await _userService.GetUsersInformationListAsync(new SearchArguments(value, page*size, size, orderBy, orderByType)));

    }
}
