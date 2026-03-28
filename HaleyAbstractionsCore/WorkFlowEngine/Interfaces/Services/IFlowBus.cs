using Haley.Enums;
using Haley.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    /// <summary>
    /// Entry-point router for workflow initiation.
    ///
    /// Business logic calls InitiateAsync to start a workflow — it has no knowledge of
    /// whether the engine or the relay is running the sequence. FlowBus resolves the
    /// executor based on GlobalMode and what is registered in the service collection.
    ///
    /// Mode resolution (when per-call Mode is null):
    ///   GlobalMode == null  → Engine ?? Relay ?? throw
    ///   GlobalMode == Engine → Engine or throw
    ///   GlobalMode == Relay  → Relay or throw
    ///
    /// Per-call Mode on FlowInitiateRequest overrides GlobalMode for that call.
    /// </summary>
    public interface IFlowBus {
        /// <summary>
        /// Global mode setting (from appsettings / DI configuration).
        /// Null means auto-resolve: Engine takes priority over Relay.
        /// </summary>
        FlowBusMode? GlobalMode { get; }

        /// <summary>
        /// Initiate a workflow for the given entity.
        /// Returns IFeedback — Status=false with Message when no executor is available or initiation fails.
        /// </summary>
        Task<IFeedback> InitiateAsync(FlowInitiateRequest request, CancellationToken ct = default);
    }
}
