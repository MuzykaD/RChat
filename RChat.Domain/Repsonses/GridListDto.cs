using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Repsonses
{
    public class GridListDto<T>
    {
        public IEnumerable<T> SelectedEntities { get; set; }
        public int TotalCount { get; set; }
    }
}
