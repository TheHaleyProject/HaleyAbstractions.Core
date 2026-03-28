namespace Haley.Enums {
    public enum FlowBusMode {
        /// <summary>Local sequential runner — no engine, no DB, no EventStore. Use WorkflowRelay.</summary>
        Relay,
        /// <summary>Full engine-backed execution via IWorkFlowEngineAccessor.TriggerAsync.</summary>
        Executor,
    }
}
