using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleRuntimeBus {
        Task<LifeCycleTriggerResult> TriggerAsync(LifeCycleTriggerRequest req, CancellationToken ct = default);
        Task ClearCacheAsync(CancellationToken ct = default);
        Task InvalidateAsync(int envCode, string defName, CancellationToken ct = default);
        Task InvalidateAsync(long defVersionId, CancellationToken ct = default);
        Task<string?> GetTimelineJsonAsync(long instanceId, CancellationToken ct = default);
        Task<IReadOnlyList<InstanceRefItem>> GetInstanceRefsAsync(int envCode, string defName, LifeCycleInstanceFlag flags, int skip, int take, CancellationToken ct = default);
        public Task<long> UpsertRuntimeAsync(RuntimeLogByNameRequest req, CancellationToken ct = default);
        public Task<int> SetRuntimeStatusAsync(long runtimeId, string status, CancellationToken ct = default);
        public Task<int> FreezeRuntimeAsync(long runtimeId, CancellationToken ct = default);
        public Task<int> UnfreezeRuntimeAsync(long runtimeId, CancellationToken ct = default);
    }
}
