using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckDAL {
        Task<DbRow?> GetByIdAsync(long ackId, DbExecutionLoad load = default);
        Task<long> UpsertByConsumerAndSourceReturnIdAsync(long consumer, long source, int ackStatus, DbExecutionLoad load = default);
        Task<int> SetStatusAsync(long ackId, int ackStatus, DbExecutionLoad load = default);
        Task<int> MarkRetryAsync(long ackId, DbExecutionLoad load = default);
    }
}
