using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Chats;
using RChat.Application.Mappers;
using RChat.Domain;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Users.DTO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

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
        [HttpGet("private/{userId}")]
        public async Task<IActionResult> GetPrivateChatByEmailAsync([FromRoute]int userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var chat = await _chatService.GetPrivateChatByUsersIdAsync(int.Parse(currentUserId), userId);
           
            return Ok(chat!.ToChatDto());
        }
    }
}
