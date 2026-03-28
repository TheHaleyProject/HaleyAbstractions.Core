using Haley.Abstractions;
using Haley.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    /// <summary>
    /// Hosted service that drives in-process workflow execution via WorkflowRelay.
    /// Register with AddWorkflowRelayService() in DI.
    /// FlowBus resolves this to initiate workflows in Relay mode.
    /// </summary>
    public interface IWorkflowRelayService {
        Task<IFeedback> InitiateAsync(FlowInitiateRequest request, CancellationToken ct = default);
    }
}
