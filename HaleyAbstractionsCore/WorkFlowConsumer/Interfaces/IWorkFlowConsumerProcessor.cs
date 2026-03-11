using System.Reflection;
using Haley.Models;

namespace Haley.Abstractions {
    //Runtime processor
    public interface IWorkFlowConsumerProcessor {
        /// <summary>
        /// Fires for every failure or notable event inside the consumer service — dispatch errors,
        /// outbox ACK failures, heartbeat errors, registry misses, etc.
        /// Mirror of <see cref="IWorkFlowEngine.NoticeRaised"/>. Subscribe to surface problems
        /// that would otherwise be silently swallowed.
        /// </summary>
        event Func<LifeCycleNotice, Task>? NoticeRaised;

        /// <summary>Scans <paramref name="assembly"/> for <see cref="LifeCycleWrapper"/> subclasses
        /// decorated with <see cref="LifeCycleDefinitionAttribute"/> and registers them.</summary>
        IWorkFlowConsumerProcessor RegisterAssembly(Assembly assembly);
        /// <summary>Loads the named assembly (if not already loaded) and scans it.</summary>
        IWorkFlowConsumerProcessor RegisterAssembly(string assemblyName);
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
