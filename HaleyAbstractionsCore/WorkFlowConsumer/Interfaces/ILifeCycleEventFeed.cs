using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    /// <summary>
    /// Abstraction over the event source. Extends <see cref="ILifeCycleConsumerBus"/> with
    /// polling methods so the consumer service can fetch queued events regardless of whether
    /// the engine is in-process or remote.
    /// </summary>
    public interface ILifeCycleEventFeed : ILifeCycleConsumerBus {
        /// <summary>
        /// Fires for feed-level failures and, when the underlying source supports it, relayed
        /// notices from that source (e.g. engine <see cref="IWorkFlowEngine.NoticeRaised"/> for
        /// <see cref="InProcessEventFeed"/>). The consumer service subscribes to this and surfaces
        /// the notices through its own <see cref="IWorkFlowConsumerService.NoticeRaised"/>.
        /// </summary>
        event Func<LifeCycleNotice, Task>? NoticeRaised;

        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueTransitionsAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueHooksAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
    }
}
