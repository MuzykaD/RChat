using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RChat.Domain
{
    public class SearchArguments
    {
        public SearchArguments(string? value, int skip, int take, string? orderBy, string? orderByType)
        {
            Value = value;
            Skip = skip;
            Take = take;
            OrderBy = orderBy;
            OrderByType = orderByType;
        }
        public string? Value { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderByType { get; set; }
        [JsonIgnore]
        public bool SearchRequired => !string.IsNullOrWhiteSpace(Value);
        [JsonIgnore]
        public bool OrderByRequired => !string.IsNullOrWhiteSpace(OrderBy) && !string.IsNullOrWhiteSpace(OrderByType);
    }
}
