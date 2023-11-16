using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Chats;
using RChat.Application.Mappers;
using RChat.Domain;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
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
        {
            var result = await _chatService.GetChatsInformationListAsync(new SearchArguments(value, page * size, size, orderBy, orderByType));
            return Ok(result);
        }
        [HttpGet("private/{userId}")]
        public async Task<IActionResult> GetPrivateChatByEmailAsync([FromRoute] int userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var chat = await _chatService.GetPrivateChatByUsersIdAsync(int.Parse(currentUserId), userId);

            return Ok(chat!.ToChatDto());
        }

        [HttpGet("group/{chatId}")]
        public async Task<IActionResult> GetGroupChatByIdAsync([FromRoute] int chatId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Chat chat = await _chatService.GetGroupChatByIdAsync(currentUserId, chatId);

            return Ok(chat!.ToChatDto());
        }

        [HttpPost("group")]
        public async Task<IActionResult> CreatePublicGroupAsync(CreateGroupChatDto createGroupDto)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var list = createGroupDto.MembersId.ToList();
            list.Add(currentUserId);
            var result = await _chatService.CreatePublicGroupAsync(createGroupDto.GroupName, currentUserId, list);
            var response = new ApiResponse() { IsSucceed = result };
            return result ? Ok(response) : BadRequest(response);
        }

        [HttpGet("group-info")]
        public async Task<IActionResult> GetUserGroupsInfo()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            IEnumerable<int> groupIds = await _chatService.GetGroupsIdentifiersAsync(currentUserId);
            return Ok(new GroupsIdentifies()
            {
                SignalIdentifiers = groupIds.ToList(),
                IsSucceed = groupIds.Any()
            });
        }
    }
}
