using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleEngine {
        event Func<ILifeCycleEvent, Task>? EventRaised;
        event Func<LifeCycleNotice, Task>? NoticeRaised;
        Task<LifeCycleTriggerResult> TriggerAsync(LifeCycleTriggerRequest req, CancellationToken ct = default);
        Task AckAsync(long consumerId, string ackGuid, AckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, CancellationToken ct = default);
        Task AckAsync(int envCode, string consumerGuid, string ackGuid, AckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, CancellationToken ct = default);
        Task<long> RegisterConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
        Task BeatConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
        Task ClearCacheAsync(CancellationToken ct = default);
        Task InvalidateAsync(int envCode, string defName, CancellationToken ct = default);
        Task InvalidateAsync(long defVersionId, CancellationToken ct = default);
        Task<string?> GetTimelineJsonAsync(long instanceId, CancellationToken ct = default);
        Task RunMonitorOnceAsync(CancellationToken ct = default);
        Task StartMonitorAsync(CancellationToken ct = default);
        Task StopMonitorAsync(CancellationToken ct = default);
    }
}
