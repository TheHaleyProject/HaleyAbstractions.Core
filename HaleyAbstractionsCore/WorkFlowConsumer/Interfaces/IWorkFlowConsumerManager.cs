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
        Task<DbRows> ListInstancesAsync(ConsumerInstanceFilter filter, CancellationToken ct = default);
        Task<DbRows> ListInboxAsync(ConsumerInboxFilter filter, CancellationToken ct = default);
        Task<DbRows> ListInboxStatusesAsync(ConsumerInboxStatusFilter filter, CancellationToken ct = default);
        Task<DbRows> ListOutboxAsync(ConsumerOutboxFilter filter, CancellationToken ct = default);
        Task<long> CountPendingInboxAsync(CancellationToken ct = default);
        Task<long> CountPendingOutboxAsync(CancellationToken ct = default);

        /// <summary>
        /// Returns the full consumer-side history for a workflow instance:
        /// every inbox event (transitions + hooks) with its processing status, outbox ACK record,
        /// outbox retry history, and business action results — all correlated by instance_guid.
        /// </summary>
        Task<ConsumerTimeline> GetConsumerTimelineAsync(string instanceGuid, CancellationToken ct = default);

        // ── Instance management (client-facing) ──────────────────────────────
        /// <summary>
        /// Upserts the consumer-side instance mirror after a successful engine trigger.
        /// Called by the service layer once it has a <see cref="LifeCycleTriggerResult"/> back from the engine.
        /// </summary>
        Task RecordInstanceAsync(string entityGuid, string defName, LifeCycleTriggerResult result, CancellationToken ct = default);

        /// <summary>
        /// Returns all instances associated with the given entity GUID across all definitions.
        /// </summary>
        Task<DbRows> GetInstancesByEntityAsync(string entityGuid, CancellationToken ct = default);

        // ── Lifecycle ─────────────────────────────────────────────────────────
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
