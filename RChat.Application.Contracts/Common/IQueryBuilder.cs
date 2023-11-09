using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Common
{
    public interface IQueryBuilder<TEntity>
    {
        public Type CurrentType { get; }
        IQueryable<TEntity> BuildSearchQuery(IQueryable<TEntity> source, string searchValues);

        IQueryable<TEntity> BuildOrderByQuery(IQueryable<TEntity> source, string orderByValue, string orderByType);
    }
}
