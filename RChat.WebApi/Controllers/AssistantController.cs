using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.AssistantFiles;
using RChat.Domain.Assistants;
using RChat.Domain.Assistants.Dto;
using RChat.Domain.Repsonses;
using System.Security.Claims;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/assistants")]
    [Authorize]
    public class AssistantController : ControllerBase
    {
        public IAssistantService AssistantService { get; set; }
        public AssistantController(IAssistantService assistantService)
        {
            AssistantService = assistantService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAssistantsAvailableAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var assistants = await AssistantService.GetAvailableAssistantsAsync(int.Parse(currentUserId));
            return Ok(new GridListDto<Assistant> { SelectedEntities = assistants, TotalCount = assistants.Count });
        }

        [HttpGet]
        public async Task<IActionResult> GetAssistantInfoByIdAsync(string assistantId)
        {
            var assistantInfo = await AssistantService.GetAssistantInfoByIdAsync(assistantId);
            return Ok(assistantInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAssistantInfoAsync([FromQuery] string assistantId, [FromBody] AssistantUpdateDto updateDto)
        {
            await AssistantService.UpdateAssistantAsync(assistantId, updateDto);
            return Ok(new ApiResponse());
        }

        [HttpPost("file")]
        public async Task<IActionResult> CreateAssistantFileAsync([FromBody] CreateAssistantFileDto createFileDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await AssistantService.CreateAssitantFileAsync(int.Parse(currentUserId), createFileDto);
            return Ok(new ApiResponse() { IsSucceed = true });
        }

        [HttpDelete("file")]
        public async Task<IActionResult> DeleAssistantFileAsync([FromQuery] string fileId)
        {
            await AssistantService.DeleteAssitantFileAsync(fileId);
            return Ok(new ApiResponse() { IsSucceed = true });
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetAssistantsFilesAttached(string assistantId)
        {
            var result = await AssistantService.GetFilesByAssistantIdAsync(assistantId);
            return Ok(new GridListDto<AssistantFile> { SelectedEntities = result, TotalCount = result.Count });
        }
    }
}
