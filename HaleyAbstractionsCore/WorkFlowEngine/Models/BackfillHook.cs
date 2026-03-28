using System;

namespace Haley.Models {
    /// <summary>
    /// One hook event within a backfill transition.
    /// </summary>
    public sealed class BackfillHook {
        public string    Route     { get; set; } = string.Empty;
        /// <summary>If null, inherits the parent BackfillTransition.Timestamp.</summary>
        public DateTime? Timestamp { get; set; }
        /// <summary>"processed", "failed", or null if outcome is unknown.</summary>
        public string?   Outcome   { get; set; }
    }
}
