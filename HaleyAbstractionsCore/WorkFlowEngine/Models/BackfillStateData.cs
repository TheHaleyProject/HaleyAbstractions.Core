using System;

namespace Haley.Models {
    /// <summary>
    /// Data returned by <see cref="Haley.Abstractions.IBackfillDataProvider.GetTransitionDataAsync"/>
    /// when the entity passed through a given event in its legacy history.
    /// </summary>
    public sealed class BackfillStateData {
        public DateTime Timestamp { get; set; }
        public string?  Actor     { get; set; }
        /// <summary>Optional JSON payload to attach to this lifecycle entry.</summary>
        public string?  Payload   { get; set; }
    }
}
