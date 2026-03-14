using System.Reflection;
using Haley.Models;

namespace Haley.Abstractions {
    //Runtime processor
    public interface IWorkFlowConsumerManager {
        /// <summary>
        /// Fires for every failure or notable event inside the consumer service — dispatch errors,
        /// outbox ACK failures, heartbeat errors, registry misses, etc.
        /// Mirror of <see cref="IWorkFlowEngine.NoticeRaised"/>. Subscribe to surface problems
        /// that would otherwise be silently swallowed.
        /// </summary>
        event Func<LifeCycleNotice, Task>? NoticeRaised;

        /// <summary>
        /// Numeric consumer ID assigned by the engine on startup via RegisterConsumerAsync.
        /// Zero until StartAsync completes. Stable for the lifetime of the runtime session.
        /// </summary>
        long ConsumerId { get; }

        /// <summary>
        /// Consumer GUID as configured in WorkFlowConsumerOptions. Stable across restarts.
        /// Paired with <see cref="ConsumerId"/> for identity checks.
        /// </summary>
        string ConsumerGuid { get; }

        /// <summary>Scans <paramref name="assembly"/> for <see cref="LifeCycleWrapper"/> subclasses
        /// decorated with <see cref="LifeCycleDefinitionAttribute"/> and registers them.</summary>
        IWorkFlowConsumerManager RegisterAssembly(Assembly assembly);
        /// <summary>Loads the named assembly (if not already loaded) and scans it.</summary>
        IWorkFlowConsumerManager RegisterAssembly(string assemblyName);

        // ── Administrative reads ──────────────────────────────────────────────
        Task<DbRows> ListWorkflowsAsync(int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListInboxAsync(int? status, int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListOutboxAsync(int? status, int skip, int take, CancellationToken ct = default);
        Task<long> CountPendingInboxAsync(CancellationToken ct = default);
        Task<long> CountPendingOutboxAsync(CancellationToken ct = default);

        // ── Entity & Workflow management (client-facing) ──────────────────────
        /// <summary>
        /// Creates a new entity row and returns its GUID. The client stores this ID as their
        /// cross-system entity reference.
        /// </summary>
        Task<string> CreateEntityAsync(CancellationToken ct = default);

        /// <summary>
        /// Records the entity→workflow mapping in the consumer DB after a successful engine trigger.
        /// Called by the service layer once it has a <see cref="LifeCycleTriggerResult"/> back from the engine.
        /// </summary>
        Task RecordEntityWorkflowAsync(string entityId, string defName, LifeCycleTriggerResult result, CancellationToken ct = default);

        /// <summary>
        /// Returns all workflows the entity is enrolled in (one row per definition it was triggered for).
        /// </summary>
        Task<DbRows> GetWorkflowsByEntityAsync(string entityId, CancellationToken ct = default);

        // ── Lifecycle ─────────────────────────────────────────────────────────
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
