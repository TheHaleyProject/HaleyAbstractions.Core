using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IHookAckDAL {
        Task<int> AttachAsync(long ackId, long hookId, DbExecutionLoad load = default);
        Task<int> DetachAsync(long ackId, long hookId, DbExecutionLoad load = default);
    }
}
