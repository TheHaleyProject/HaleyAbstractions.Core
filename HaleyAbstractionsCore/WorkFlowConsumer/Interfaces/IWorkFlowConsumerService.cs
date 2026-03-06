using System.Reflection;

namespace Haley.Abstractions {
    public interface IWorkFlowConsumerService {
        /// <summary>Scans <paramref name="assembly"/> for <see cref="LifeCycleWrapper"/> subclasses
        /// decorated with <see cref="LifeCycleDefinitionAttribute"/> and registers them.</summary>
        IWorkFlowConsumerService RegisterAssembly(Assembly assembly);
        /// <summary>Loads the named assembly (if not already loaded) and scans it.</summary>
        IWorkFlowConsumerService RegisterAssembly(string assemblyName);
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
