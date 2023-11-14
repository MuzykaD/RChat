using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Chats;
using RChat.Domain;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Users.DTO;
using System.Linq;
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
            var result = new ChatDto()
            {
                Id = chat.Id,
                CreatorId = chat.CreatorId,
                Name = chat.Name,
                Messages = chat.Messages.Select(m => new Domain.Messages.Dto.MessageInformationDto()
                {
                    Id = m.Id,
                    ChatId = chat.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                }).ToList(),
                Users = chat.Users.Select(u => new Domain.Users.DTO.UserInformationDto()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                }).ToHashSet()
            };
            return Ok(result);
        }
    }
}
