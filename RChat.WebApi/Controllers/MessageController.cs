using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Messages;
using RChat.Domain;
using RChat.Domain.Messages.Dto;
using System.Security.Claims;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsersInformation([FromQuery] int page, int size, string? value, string? orderBy, string? orderByType)
                => Ok(await _messageService.GetMessagesInformationListAsync(new SearchArguments(value, page * size, size, orderBy, orderByType)));

        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync([FromBody] CreateMessageDto messageDto)
        {
            var result = await _messageService.CreateMessageAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), messageDto);
            return result ? Ok(result) : BadRequest(result);
        }
    }
}
