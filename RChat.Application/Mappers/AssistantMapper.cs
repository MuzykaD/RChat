using RChat.Domain.Assistants;
using RChat.Domain.Assistants.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Mappers
{
    public static class AssistantMapper
    {
        public static AssistantInfoDto ToAssistantInfoDto(this Domain.Assistants.Assistant assistant)
        {
            return new()
            {
                Name = assistant.Name,
                Context = assistant.Instructions,
                ChatName = assistant.Chat.Name,
                FilesCount = assistant.AssistantFiles.Count,
            };
        }
    }
}
