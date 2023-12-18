using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Assistant;

namespace RChat.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/upload")]
    public class UploadController : ControllerBase
    {
        public IAssistantFileService AssistantFileService { get; set; }
        public UploadController(IAssistantFileService assistantFileService)
        {
            AssistantFileService = assistantFileService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileToAssistantAsync(IFormFile file)
        {

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            await AssistantFileService.AddFileAsync(file.FileName, fileBytes, "user");

            return Ok(true);
        }
    }
}
