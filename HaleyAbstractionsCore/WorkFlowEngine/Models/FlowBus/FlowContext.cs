namespace Haley.Models {
    /// <summary>
    /// Caller-provided context passed to IFlowBus.NextAsync.
    /// Identifies the entity and workflow being advanced.
    /// </summary>
    public sealed class FlowContext {
        public string  EntityRef    { get; set; } = string.Empty;
        public string  WorkflowName { get; set; } = string.Empty;
        public int     EnvCode      { get; set; }
        public string? Actor        { get; set; }
        public object? Payload      { get; set; }
    }
}
