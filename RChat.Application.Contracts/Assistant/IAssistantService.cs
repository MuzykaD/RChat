
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantService
    {
        public Task<List<RChat.Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId);
    }
}
