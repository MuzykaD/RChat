using System.Text;

namespace RChat.UI.Common
{
    internal static class HttpQueryBuilder
    {
        internal static string BuildGridListQuery(int? page, int size, string value, string? orderBy, string? orderByType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"?page={page}&size={size}");
            if (!string.IsNullOrWhiteSpace(value))  
                sb.Append("&value=" + value);
            if (!string.IsNullOrWhiteSpace(orderBy) && !string.IsNullOrWhiteSpace(orderByType))
            {
                sb.Append("&orderBy=" + orderBy);
                sb.Append("&orderByType=" + orderByType);
            }
            return sb.ToString();

        }
    }
}
