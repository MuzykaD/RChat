using LinqKit;
using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Users;
using RChat.Domain.Users;
using System.Linq.Expressions;
using System.Reflection;

namespace RChat.Application.Common
{
    public class QueryBuilder<TEntity> : IQueryBuilder<TEntity>
    {
        public Expression<Func<TEntity, bool>> SearchQuery<TEntity>(string searchValues, params string[] namesOfProperties)
        {        
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            
            Expression expression = null;
            foreach (var name in namesOfProperties)
            {
                var property = Expression.Property(parameter, name);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsCall = Expression.Call(property, containsMethod, Expression.Constant(searchValues));
                if (expression == null)
                {
                    expression = containsCall;
                }
                else
                {
                    expression = Expression.OrElse(expression, containsCall);
                }
            }
            return Expression.Lambda<Func<TEntity, bool>>(expression!, parameter);
        }

        public IQueryable<TEntity> OrderByQuery<TEntity>(IQueryable<TEntity> source, string orderByValue, string orderByType)
        {
            string command = orderByType.Equals("Descending", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByValue);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }


    }
}
