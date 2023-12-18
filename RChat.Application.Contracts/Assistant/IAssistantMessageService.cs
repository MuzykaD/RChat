using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantMessageService : IAssistantInitializer
    {
        public Task<string> SendMessageAndGetResponseAsync(string message);
    }
}
