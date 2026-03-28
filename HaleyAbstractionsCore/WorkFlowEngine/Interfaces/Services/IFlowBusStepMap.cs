namespace Haley.Abstractions {
    /// <summary>
    /// Application-registered map that resolves an outcome string to the workflow event
    /// that should be triggered next.
    ///
    /// Each application implements and registers its own step map at startup.
    /// This keeps Haley infrastructure generic — routing decisions stay in the application.
    ///
    /// Executor mode: outcome → event name (engine resolves name to code internally).
    /// WorkflowRelay: outcome → event code (int) — codes are the stable contract.
    /// </summary>
    public interface IFlowBusStepMap {
        /// <summary>
        /// Resolve outcome → workflow event name for Executor mode.
        /// Return null if this workflow+outcome combination has no mapping.
        /// </summary>
        string? GetExecutorEventName(string workflowName, string? outcome);

        /// <summary>
        /// Resolve outcome → event code for WorkflowRelay (local runner) mode.
        /// Return null if this workflow+outcome combination has no mapping.
        /// </summary>
        int? GetRelayEventCode(string workflowName, string? outcome);
    }
}
