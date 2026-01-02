using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILcAckDAL {
        Task<int> AttachAsync(long ackId, long lcId, DbExecutionLoad load = default);
        Task<int> DetachAsync(long ackId, long lcId, DbExecutionLoad load = default);
    }
}