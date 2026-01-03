using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleEngine {
        event Func<ILifeCycleEvent, Task>? EventRaised;
        Task<LifeCycleTriggerResult> TriggerAsync(LifeCycleTriggerRequest req, CancellationToken ct = default);
        Task AckAsync(long consumerId, string ackGuid, LifeCycleAckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, CancellationToken ct = default);
        Task ClearCacheAsync(CancellationToken ct = default);
        Task InvalidateAsync(int envCode, string defName, CancellationToken ct = default);
        Task InvalidateAsync(long defVersionId, CancellationToken ct = default);
    }
}
