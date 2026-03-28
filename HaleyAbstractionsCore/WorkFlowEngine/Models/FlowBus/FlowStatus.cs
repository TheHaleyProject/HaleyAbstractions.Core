namespace Haley.Models {
    /// <summary>
    /// Current workflow status for an entity. Returned by IFlowBus.GetStatusAsync.
    /// Only meaningful in Executor mode — null is returned in Relay/WorkflowRelay mode.
    /// </summary>
    public sealed class FlowStatus {
        public string CurrentState { get; init; } = string.Empty;
        public bool   IsCompleted  { get; init; }
        public bool   IsSuspended  { get; init; }
        public string InstanceGuid { get; init; } = string.Empty;
    }
}
