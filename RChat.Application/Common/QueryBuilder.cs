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
        public Type CurrentType => typeof(TEntity);
       
        public IQueryable<TEntity> BuildSearchQuery(IQueryable<TEntity> source, string searchValues)
        {
            var properties = GetTypeStringProperties();
            var parameter = Expression.Parameter(CurrentType, "x");
            Expression expression = null;
            foreach (var name in properties)
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
            return source.Where(Expression.Lambda<Func<TEntity, bool>>(expression!, parameter));
        }

        public IQueryable<TEntity> BuildOrderByQuery(IQueryable<TEntity> source, string orderByValue, string orderByType)
        {
            string command = orderByType.Equals("Descending", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByValue);
            if(property == null)
                return source;
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        private string[] GetTypeStringProperties()
        {
            return CurrentType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !ForbiddenPatterns.Columns.Any(forbiden => p.Name.Contains(forbiden)))
                .Select(p => p.Name)
                .ToArray();
        }       
    }
}
