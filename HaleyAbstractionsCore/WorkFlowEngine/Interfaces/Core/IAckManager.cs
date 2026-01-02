using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckManager {
        Task AckAsync(long ackId, LifeCycleAckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, DbExecutionLoad load = default);

        // monitor pulls events that must be re-raised (pending/unacked)
        Task<IReadOnlyList<ILifeCycleEvent>> GetPendingDispatchAsync(DateTime utcNow, int take, DbExecutionLoad load = default);
    }
}
