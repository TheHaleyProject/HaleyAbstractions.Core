using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckManager {
        Task<IReadOnlyList<long>> GetTransitionConsumersAsync(long defVersionId, long instanceId, CancellationToken ct = default);
        Task<IReadOnlyList<long>> GetHookConsumersAsync(long defVersionId, long instanceId, string hookCode, CancellationToken ct = default);
        Task<ILifeCycleAckRef> CreateLifecycleAckAsync(long lifecycleId, IReadOnlyList<long> consumerIds, int initialAckStatus, DbExecutionLoad load = default);
        Task<ILifeCycleAckRef> CreateHookAckAsync(long hookId, IReadOnlyList<long> consumerIds, int initialAckStatus, DbExecutionLoad load = default);
        Task AckAsync(long consumerId, string ackGuid, AckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, DbExecutionLoad load = default);
        Task MarkRetryAsync(long ackId, long consumerId, DateTimeOffset? retryAt = null, DbExecutionLoad load = default);
        Task SetStatusAsync(long ackId, long consumerId, int ackStatus, DbExecutionLoad load = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> ListDueLifecycleDispatchAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, DbExecutionLoad load = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> ListDueHookDispatchAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, DbExecutionLoad load = default);
        Task<int> CountDueLifecycleDispatchAsync(int ackStatus, DbExecutionLoad load = default);
        Task<int> CountDueHookDispatchAsync(int ackStatus, DbExecutionLoad load = default);
    }

}
