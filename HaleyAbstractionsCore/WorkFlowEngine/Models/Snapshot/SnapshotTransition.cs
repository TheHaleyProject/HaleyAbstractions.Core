using System.Collections.Generic;

namespace Haley.Models {
    public sealed class SnapshotTransition {
        public string FromState { get; init; } = string.Empty;
        public string ToState   { get; init; } = string.Empty;
        /// <summary>Stable numeric event code. Use this when building BackfillTransition.EventCode.</summary>
        public int    EventCode { get; init; }
        /// <summary>Human-readable event name — for display only, may change over time.</summary>
        public string EventName { get; init; } = string.Empty;
        /// <summary>Auto-advance event code when the transition handler returns true (success).</summary>
        public int?   CompleteSuccessCode { get; init; }
        /// <summary>Auto-advance event code when the transition handler returns false (failure).</summary>
        public int?   CompleteFailureCode { get; init; }
        public IReadOnlyList<SnapshotHookRoute> Hooks { get; init; } = System.Array.Empty<SnapshotHookRoute>();
    }
}
