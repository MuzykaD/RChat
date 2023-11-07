using System.Text;

namespace RChat.UI.Common
{
    internal static class HttpQueryBuilder
    {
        internal static string BuildGridListQuery(int? skip, int take, string searchValue, string? orderBy, string? orderByType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"?skip={skip}&take={take}");
            if (!string.IsNullOrWhiteSpace(searchValue))  
                sb.Append("&value=" + searchValue);
            if (!string.IsNullOrWhiteSpace(orderBy) && !string.IsNullOrWhiteSpace(orderByType))
            {
                sb.Append("&orderBy=" + orderBy);
                sb.Append("&orderByType=" + orderByType);
            }
            return sb.ToString();

        }
    }
}
