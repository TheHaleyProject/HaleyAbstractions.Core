using Haley.Enums;
using Haley.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    /// <summary>
    /// Seam between business logic and the workflow sequencing mechanism.
    ///
    /// Business methods call NextAsync after completing a unit of work — they do not
    /// know what comes next. FlowBus owns the sequence.
    ///
    /// Two implementations:
    ///   Relay    → WorkflowRelay (local, zero infra, no engine required)
    ///   Executor → ExecutorFlowBus (engine-backed, full persistence)
    ///
    /// Switching from Relay to Executor requires no business logic changes — only the
    /// registered IFlowBus implementation changes.
    /// </summary>
    public interface IFlowBus {
        FlowBusMode Mode { get; }

        /// <summary>
        /// Advance the workflow sequence after completing a unit of work.
        /// outcome selects the transition path when a state has multiple exits.
        /// Returns FlowBusResult — Applied=false with Reason when blocked, never throws for flow reasons.
        /// </summary>
        Task<FlowBusResult> NextAsync(FlowContext ctx, string? outcome = null, CancellationToken ct = default);

        /// <summary>
        /// Returns current workflow status for the entity.
        /// Returns null in Relay/WorkflowRelay mode (no persistence).
        /// </summary>
        Task<FlowStatus?> GetStatusAsync(FlowContext ctx, CancellationToken ct = default);
    }
}
