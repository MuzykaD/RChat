﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Common
{
    public interface IDbEntity<T>
    {
        public T Id { get; set; }
    }
}
