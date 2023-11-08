using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Chats;
using RChat.Domain;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/chats")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatsInformation([FromQuery] int page, int size, string? value, string? orderBy, string? orderByType)
                => Ok(await _chatService.GetChatsInformationListAsync(new SearchArguments(value, page * size, size, orderBy, orderByType)));
    }
}
