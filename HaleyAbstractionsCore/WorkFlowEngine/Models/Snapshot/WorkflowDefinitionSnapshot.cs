using System.Collections.Generic;

namespace Haley.Models {
    /// <summary>
    /// Read-only projection of a workflow definition returned by the engine so consumers
    /// can inspect valid states, transitions, and hook routes without accessing the engine DB directly.
    /// Used by WorkflowBackfillValidator to validate backfill objects client-side.
    /// </summary>
    public sealed class WorkflowDefinitionSnapshot {
        public string DefinitionName { get; init; } = string.Empty;
        public int    EnvCode        { get; init; }
        public IReadOnlyList<SnapshotState>      States      { get; init; } = System.Array.Empty<SnapshotState>();
        public IReadOnlyList<SnapshotTransition> Transitions { get; init; } = System.Array.Empty<SnapshotTransition>();
        /// <summary>Top-level parameter catalog from the policy JSON params array. Keyed by Code for quick lookup.</summary>
        public IReadOnlyList<SnapshotParameter>  Parameters  { get; init; } = System.Array.Empty<SnapshotParameter>();
    }
}
