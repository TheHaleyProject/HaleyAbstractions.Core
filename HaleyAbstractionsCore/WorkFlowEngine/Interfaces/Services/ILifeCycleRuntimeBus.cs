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
        Task<string?> GetTimelineJsonAsync(LifeCycleInstanceKey key, TimelineDetail detail = TimelineDetail.Detailed, CancellationToken ct = default);
        Task<IReadOnlyList<InstanceRefItem>> GetInstanceRefsAsync(int envCode, string defName, LifeCycleInstanceFlag flags, int skip, int take, CancellationToken ct = default);
        // Call once to record the initial status ("running"), call again with the final status ("approved",
        // "rejected", etc.) — the upsert updates the existing row in-place. No separate SetStatus needed.
        Task<long> UpsertRuntimeAsync(RuntimeLogByNameRequest req, CancellationToken ct = default);
        Task<DbRows> ListInstancesAsync(int envCode, string? defName, bool runningOnly, int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListInstancesByStatusAsync(int envCode, string? defName, LifeCycleInstanceFlag statusFlags, int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListPendingAcksAsync(int envCode, int skip, int take, CancellationToken ct = default);
        // Suspends an in-progress instance by setting Suspended and clearing Active.
        // This does not perform any transition and keeps current_state unchanged.
        Task<bool> SuspendInstanceAsync(string instanceGuid, string? message, CancellationToken ct = default);
        // Resumes a suspended instance by clearing Suspended and restoring Active.
        // This continues from current_state and does not trigger auto-start.
        Task<bool> ResumeInstanceAsync(string instanceGuid, CancellationToken ct = default);
        // Marks an instance as failed by setting the Failed instance flag only.
        // This does not perform any lifecycle transition and does not change current_state.
        Task<bool> FailInstanceAsync(string instanceGuid, string? message, CancellationToken ct = default);
        // Resets a terminal (Completed/Failed/Archived) instance back to its initial state and immediately
        // fires the auto-start event so it begins a fresh run. Useful for reopen and idempotency testing.
        // Returns Applied=false with Reason="NotTerminal" if the instance is not in a terminal state.
        Task<LifeCycleTriggerResult> ReopenAsync(string instanceGuid, string actor, CancellationToken ct = default);
        // Extends the retry budget of all Failed ack_consumer rows for the instance
        // (max_trigger = trigger_count + globalMaxRetryCount) and clears the Suspended flag.
        // Use this when an instance was suspended because its ACK retry count was exhausted.
        // trigger_count is never reset — it remains a monotonically increasing audit counter.
        // Returns false if the instance does not exist; throws if it is not in the Suspended state.
        Task<bool> UnsuspendAsync(string instanceGuid, string actor, CancellationToken ct = default);
    }
}


