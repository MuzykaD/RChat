using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Messages;
using RChat.Domain;

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
    }
}
