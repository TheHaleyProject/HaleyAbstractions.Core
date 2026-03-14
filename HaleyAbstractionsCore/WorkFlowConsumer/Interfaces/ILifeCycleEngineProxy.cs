using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    /// <summary>
    /// Full proxy abstraction between the consumer infrastructure and wherever the engine lives.
    /// Extends <see cref="ILifeCycleExecution"/> (the wrapper-facing surface) with the additional
    /// polling methods used exclusively by the consumer's internal dispatch loop.
    ///
    /// Today: <see cref="InProcessEngineProxy"/> wires it to an in-process engine.
    /// Tomorrow: an HttpEngineProxy would POST/GET a remote engine API instead.
    /// The consumer service is entirely unaware of which implementation is active.
    ///
    /// Wrappers (<see cref="LifeCycleWrapper"/> subclasses) receive only <see cref="ILifeCycleExecution"/>,
    /// not this interface. That keeps GetDueTransitionsAsync / GetDueHooksAsync out of reach of
    /// wrapper code, preventing accidental draining of the delivery channels.
    ///
    /// Intentionally excludes engine-management operations
    /// (StartMonitorAsync, GetHealthAsync, etc.) — those are engine-internal concerns.
    /// </summary>
    public interface ILifeCycleEngineProxy : ILifeCycleExecution {
        /// <summary>
        /// Fires for proxy-level notices and, when the underlying source supports it, relayed
        /// engine notices (e.g. engine.NoticeRaised for in-process proxy).
        /// Subscribe here; do not subscribe to the engine directly from the consumer.
        /// </summary>
        event Func<LifeCycleNotice, Task>? NoticeRaised;

        // ── Polling — consumer dispatch loop only ────────────────────────────────
        // Fetches events queued for this consumer. In-process: drains an in-memory channel.
        // Remote: queries the engine's outbox API with skip/take paging.
        // These must NOT be called from wrappers — they drain the delivery pipeline.
        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueTransitionsAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueHooksAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
    }
}
