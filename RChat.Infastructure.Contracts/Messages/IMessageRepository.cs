using RChat.Domain.Messages;
using RChat.Infrastructure.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Contracts.Messages
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
