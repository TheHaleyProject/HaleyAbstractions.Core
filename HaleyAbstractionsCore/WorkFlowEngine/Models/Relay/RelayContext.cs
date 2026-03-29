using System.Collections.Generic;

namespace Haley.Models {
    /// <summary>
    /// Context passed to WorkflowRelay handler delegates.
    /// CurrentState is maintained by the runner and updated after each successful transition.
    /// Parameters holds the resolved SnapshotParameter entries for the current transition —
    /// look these up to know who can act, what approval rule applies, what role is required, etc.
    /// </summary>
    public sealed class RelayContext {
        public string  EntityRef    { get; set; } = string.Empty;
        public string  CurrentState { get; set; } = string.Empty;
        public string? Actor        { get; set; }
        public object? Payload      { get; set; }
        /// <summary>Parameters resolved for the current transition from the policy catalog. Empty if the transition has no param references.</summary>
        public IReadOnlyList<SnapshotParameter> Parameters { get; set; } = System.Array.Empty<SnapshotParameter>();

        public override string ToString() => $"{EntityRef} @ {CurrentState}{(Actor != null ? $" by {Actor}" : "")}";
    }
}
