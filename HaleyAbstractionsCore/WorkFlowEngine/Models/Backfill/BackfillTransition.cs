using System;
using System.Collections.Generic;

namespace Haley.Models {
    /// <summary>
    /// One state transition in the backfill history.
    /// </summary>
    public sealed class BackfillTransition {
        public string   FromState { get; set; } = string.Empty;
        public string   ToState   { get; set; } = string.Empty;
        /// <summary>
        /// The event code (int) that drove this transition. Use the code, not the event name —
        /// codes are the stable contract; names are display text that can be renamed.
        /// </summary>
        public int      EventCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string?  Actor     { get; set; }
        /// <summary>Optional JSON payload for this lifecycle entry.</summary>
        public string?  Payload   { get; set; }

        /// <summary>
        /// Hooks that fired during this transition, if known.
        /// Leave empty if the legacy system did not track hook-level events.
        /// </summary>
        public List<BackfillHook> Hooks { get; set; } = new();
    }
}
