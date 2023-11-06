using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Common
{
    public interface IQueryBuilder<T>
    {
        Expression<Func<User, bool>> SearchQuery(string searchValues);
    }
}
