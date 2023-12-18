using HigLabo.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantFileService : IAssistantInitializer
    {
        public Task<List<FileObject>> GetAllFilesAsync();
        public Task AddFileAsync(string fileName, byte[] fileBytes, string purpose);
        public Task RemoveFileAsync(string fileId);
    }
}
