namespace Haley.Enums {
    public enum FlowBusMode {
        /// <summary>Full engine-backed execution — WorkflowConsumerService creates and drives the instance.</summary>
        Engine,
        /// <summary>Local in-process runner — WorkflowRelayService drives the instance, zero infrastructure.</summary>
        Relay,
    }
}
