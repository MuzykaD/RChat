using RChat.Application.Contracts.Common;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Users
{
    public interface IUserQueryBuilder : IQueryBuilder<User>
    {
        
    }
}
