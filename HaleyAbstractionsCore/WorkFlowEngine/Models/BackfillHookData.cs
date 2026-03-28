using System;

namespace Haley.Models {
    /// <summary>
    /// Data returned by <see cref="Haley.Abstractions.IBackfillDataProvider.GetHookDataAsync"/>
    /// when the legacy system tracked a specific hook firing.
    /// </summary>
    public sealed class BackfillHookData {
        /// <summary>If null, inherits the parent transition's timestamp.</summary>
        public DateTime? Timestamp { get; set; }
        /// <summary>"processed", "failed", or null if outcome is unknown.</summary>
        public string?   Outcome   { get; set; }
    }
}
