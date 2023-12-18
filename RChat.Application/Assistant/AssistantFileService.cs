﻿using HigLabo.OpenAI;
using Microsoft.Extensions.Configuration;
using RChat.Application.Contracts.Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public class AssistantFileService :BaseAssistantService, IAssistantFileService
    {
        public AssistantFileService(IConfiguration configuration)
        {
            _assistantApiKey = configuration["AssistantsApiKey"]!;
            OpenAIClient = new OpenAIClient(_assistantApiKey);
        }
        public async Task AddFileAsync(string fileName, byte[] fileBytes, string purpose)
        {
            if (string.IsNullOrWhiteSpace(_currentThreadId))
                await CreateThreadAsync();

            var uploadParameter = new FileUploadParameter()
            {
                Purpose = purpose,
            };
            uploadParameter.SetFile(fileName, fileBytes);
            var res = await OpenAIClient.FileUploadAsync(uploadParameter);
        }

        public async Task<List<FileObject>> GetAllFilesAsync()
        {
            var files = await OpenAIClient.FilesAsync();
            return files.Data;
        }

        public async Task RemoveFileAsync(string fileId)
        {           
            var removeFileParameter = new AssistantFileDeleteParameter()
            {
                Assistant_Id = _currentAssistantId!,
                File_Id = fileId,
            };
            await OpenAIClient.AssistantFileDeleteAsync(removeFileParameter);
        }

        public async Task InitializeAsync(string assistantId)
        {
            var all = await OpenAIClient.AssistantsAsync();
            _currentAssistantId = all.Data.FirstOrDefault(a => a.Id == assistantId)!.Id;
            if (_currentAssistantId is null)
                throw new ArgumentException("Assistant for this chat is not created yet");
            else
                IsInitialized = true;
        }
    }
}
