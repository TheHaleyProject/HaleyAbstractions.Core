namespace Haley.Models {
    /// <summary>
    /// Context passed to WorkflowRelay handler delegates.
    /// CurrentState is maintained by the runner and updated after each successful transition.
    /// </summary>
    public sealed class RelayContext {
        public string  EntityRef    { get; set; } = string.Empty;
        public string  CurrentState { get; set; } = string.Empty;
        public string? Actor        { get; set; }
        public object? Payload      { get; set; }
    }
}
