using System.Collections.Generic;

namespace Haley.Models {
    /// <summary>
    /// A named parameter entry from the policy JSON top-level params catalog.
    /// Code is the unique key (e.g. "PARAMS.LOAN.MANAGER.APPROVAL").
    /// Data holds the raw parameter payload — approval rules, role selectors, thresholds, etc.
    /// The application reads Data to decide who can act, what rule applies, etc.
    /// </summary>
    public sealed class SnapshotParameter {
        public string Code { get; init; } = string.Empty;
        public IReadOnlyDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>();

        public override string ToString() => $"{Code} [{Data.Count} keys]";
    }
}
