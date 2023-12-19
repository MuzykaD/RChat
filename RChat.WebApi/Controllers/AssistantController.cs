﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.AssistantFiles;
using RChat.Domain.Assistants;
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

        [HttpGet]
        public async Task<IActionResult> GetAssistantsAvailableAsync() 
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var assistants = await AssistantService.GetAvailableAssistantsAsync(int.Parse(currentUserId));
            return Ok(new GridListDto<Assistant> { SelectedEntities = assistants, TotalCount = assistants.Count});
        }
        

        [HttpPost("file")]
        public async Task<IActionResult> CreateAssistantFileAsync([FromBody] CreateAssistantFileDto createFileDto)
        {         
            await AssistantService.CreateAssitantFileAsync(createFileDto);
            return Ok(new ApiResponse() { IsSucceed = true});
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
           var result =  await AssistantService.GetFilesByAssistantIdAsync(assistantId);
            return Ok(new GridListDto<AssistantFile> { SelectedEntities = result, TotalCount = result.Count});
        }
    }
}