using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    /// <summary>
    /// Proxy abstraction over the workflow engine, used by the consumer service.
    /// Extends <see cref="ILifeCycleConsumerBus"/> (register, heartbeat, ACK) with
    /// polling methods so the consumer service can fetch queued events regardless of
    /// whether the engine is in-process (<see cref="InProcessEngineProxy"/>) or remote.
    /// </summary>
    public interface ILifeCycleEngineProxy : ILifeCycleConsumerBus {
        /// <summary>
        /// Fires for proxy-level failures and, when the underlying source supports it, relayed
        /// notices from that source (e.g. engine <see cref="IWorkFlowEngine.NoticeRaised"/> for
        /// <see cref="InProcessEngineProxy"/>). The consumer service subscribes to this and surfaces
        /// the notices through its own <see cref="IWorkFlowConsumerProcessor.NoticeRaised"/>.
        /// </summary>
        event Func<LifeCycleNotice, Task>? NoticeRaised;

        
        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueTransitionsAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
        Task<IReadOnlyList<ILifeCycleDispatchItem>> GetDueHooksAsync(long consumerId, int ackStatus, int ttlSeconds, int skip, int take, CancellationToken ct = default);
    }
}
