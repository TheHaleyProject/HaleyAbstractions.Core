using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleRuntimeBus {
        Task<LifeCycleTriggerResult> TriggerAsync(LifeCycleTriggerRequest req, CancellationToken ct = default);
        Task<LifeCycleInstanceData?> GetInstanceDataAsync(LifeCycleInstanceKey key, CancellationToken ct = default);
        Task<string?> GetInstanceContextAsync(LifeCycleInstanceKey key, CancellationToken ct = default);
        Task<int> SetInstanceContextAsync(LifeCycleInstanceKey key, string? context, CancellationToken ct = default);
        Task ClearCacheAsync(CancellationToken ct = default);
        Task InvalidateAsync(int envCode, string defName, CancellationToken ct = default);
        Task InvalidateAsync(long defVersionId, CancellationToken ct = default);
        Task<string?> GetTimelineJsonAsync(LifeCycleInstanceKey key, CancellationToken ct = default);
        Task<IReadOnlyList<InstanceRefItem>> GetInstanceRefsAsync(int envCode, string defName, LifeCycleInstanceFlag flags, int skip, int take, CancellationToken ct = default);
        Task<long> UpsertRuntimeAsync(RuntimeLogByNameRequest req, CancellationToken ct = default);
        Task<int> SetRuntimeStatusAsync(LifeCycleRuntimeRef runtimeRef, string status, CancellationToken ct = default);
        Task<int> FreezeRuntimeAsync(LifeCycleRuntimeRef runtimeRef, CancellationToken ct = default);
        Task<int> UnfreezeRuntimeAsync(LifeCycleRuntimeRef runtimeRef, CancellationToken ct = default);
        Task<DbRows> ListInstancesAsync(int envCode, string? defName, bool runningOnly, int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListPendingAcksAsync(int envCode, int skip, int take, CancellationToken ct = default);
    }
}
