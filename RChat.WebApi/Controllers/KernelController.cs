using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;

using System.Security.Claims;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using RChat.Application.Contracts.ChatKernel;
using Microsoft.AspNetCore.Authorization;
using RChat.Domain.Users.DTO;
using RChat.Domain.Repsonses;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/kernel")]
    public class KernelController : ControllerBase
    {
        IChatKernelService _chatKernelService;
        public KernelController(IChatKernelService chatKernelService)
        {
            _chatKernelService = chatKernelService;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessageToUser(string prompt)
        {          
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            prompt += $"\n My Id: {currentUserId}";
            var result = await _chatKernelService.SendMessageToKernelAsync(prompt);
            return Ok(new ApiResponse() { Message = result.Content, IsSucceed = true});
        }
    }
}
