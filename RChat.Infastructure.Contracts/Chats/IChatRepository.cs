using RChat.Domain.Chats;
using RChat.Infrastructure.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Contracts.Chats
{
    public interface IChatRepository : IRepository<Chat>
    {
    }
}
