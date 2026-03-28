using Haley.Enums;

namespace Haley.Models {
    /// <summary>
    /// Parameters for IFlowBus.InitiateAsync.
    /// Mode overrides the FlowBus global mode for this specific call.
    /// When null, the FlowBus global mode decision applies (Engine ?? Relay ?? throw).
    /// </summary>
    public sealed class FlowInitiateRequest {
        public string  WorkflowName { get; set; } = string.Empty;
        public string  EntityId     { get; set; } = string.Empty;
        public int     EnvCode      { get; set; }
        public string? Actor        { get; set; }
        public object? Payload      { get; set; }
        /// <summary>Start event code or name to fire on the engine (e.g. "2000"). Required for engine mode.</summary>
        public string? StartEvent { get; set; }
        /// <summary>Per-call mode override. Null = use FlowBus.GlobalMode logic.</summary>
        public FlowBusMode? Mode    { get; set; }
    }
}
