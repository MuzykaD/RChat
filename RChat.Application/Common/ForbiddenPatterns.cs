using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Common
{
    internal class ForbiddenPatterns
    {
        internal static string[] Columns = new string[]
        {
            "Password", "Hash", "Security", "Stamp", "Salt", "Secret", "Url"
        };
    }
}
