using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckManager {
        Task<ILifeCycleAckRef> CreateLifecycleAckAsync(long lifecycleId, IReadOnlyList<long> consumerIds, int initialAckStatus, DbExecutionLoad load = default);
        Task<ILifeCycleAckRef> CreateHookAckAsync(long hookId, IReadOnlyList<long> consumerIds, int initialAckStatus, DbExecutionLoad load = default);
        Task AckAsync(long consumerId, string ackGuid, LifeCycleAckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, DbExecutionLoad load = default);
        Task MarkRetryAsync(long ackId, long consumerId, DateTimeOffset? retryAt = null, DbExecutionLoad load = default);
        Task SetStatusAsync(long ackId, long consumerId, int ackStatus, DbExecutionLoad load = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> ListPendingLifecycleDispatchAsync(long consumerId, int ackStatus, DateTime utcOlderThan, int skip, int take, DbExecutionLoad load = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> ListPendingHookDispatchAsync(long consumerId, int ackStatus, DateTime utcOlderThan, int skip, int take, DbExecutionLoad load = default);
        Task<int> CountPendingLifecycleDispatchAsync(int ackStatus, DateTime utcOlderThan, DbExecutionLoad load = default);
        Task<int> CountPendingHookDispatchAsync(int ackStatus, DateTime utcOlderThan, DbExecutionLoad load = default);
        Task<IReadOnlyList<long>> GetTransitionConsumersAsync(long defVersionId, long instanceId, CancellationToken ct = default);
        Task<IReadOnlyList<long>> GetHookConsumersAsync(long defVersionId, long instanceId, string hookCode, CancellationToken ct = default);
    }
}
