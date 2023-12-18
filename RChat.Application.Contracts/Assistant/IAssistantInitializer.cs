using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantInitializer
    {
        Task InitializeAsync(string assistantId);
    }
}
