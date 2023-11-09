using LinqKit;
using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Users;
using RChat.Domain.Users;
using System.Linq.Expressions;
using System.Reflection;

namespace RChat.Application.Common
{
    public static class QueryBuilder<TEntity>
    {
        
       
        public static IQueryable<TEntity> BuildSearchQuery(IQueryable<TEntity> source, string searchValues)
        {
            var properties = GetTypeStringProperties(typeof(TEntity));
            var parameter = Expression.Parameter(typeof(TEntity), "x");
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

        public static IQueryable<TEntity> BuildOrderByQuery(IQueryable<TEntity> source, string orderByValue, string orderByType)
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

        private static string[] GetTypeStringProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => 
                       p.PropertyType == typeof(string) 
                       && !ForbiddenPatterns.Columns
                       .Any(forbidden => p.Name.Contains(forbidden, StringComparison.OrdinalIgnoreCase)))
                .Select(p => p.Name)
                .ToArray();
        }       
    }
}
