using System.Collections.Generic;

namespace Haley.Models {
    /// <summary>
    /// Consumer-prepared representation of an entity's historical workflow journey.
    /// Built from legacy data and validated client-side by WorkflowBackfillValidator before
    /// being sent to the engine via ImportBackfillAsync.
    ///
    /// Rules:
    ///   - Transitions must be in chronological order.
    ///   - Each (FromState, ToState, EventCode) triple must match the definition snapshot.
    ///   - Hooks are optional; include only what the legacy system tracked.
    ///   - Engine rejects objects where Validated == false.
    /// </summary>
    public sealed class WorkflowBackfillObject {
        public string  WorkflowName { get; set; } = string.Empty;
        public int     EnvCode      { get; set; }
        public string  EntityRef    { get; set; } = string.Empty;
        public string? Metadata     { get; set; }

        public List<BackfillTransition> Transitions { get; set; } = new();

        // Set by WorkflowBackfillValidator after successful validation.
        // Engine refuses objects where this is false.
        public bool Validated { get; private set; }

        // Called only by WorkflowBackfillValidator — not by application code directly.
        public void MarkValidated() => Validated = true;
    }
}
