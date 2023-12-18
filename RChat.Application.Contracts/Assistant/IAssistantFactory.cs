using HigLabo.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantFactory
    {
        public Task<AssistantCreateResponse> CreateAssistantForGroupAsync(string groupName);
    }
}
