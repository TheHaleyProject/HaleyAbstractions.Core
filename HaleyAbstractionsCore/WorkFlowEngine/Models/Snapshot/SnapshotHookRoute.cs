namespace Haley.Models {
    public sealed class SnapshotHookRoute {
        public string Route    { get; init; } = string.Empty;
        public string Label    { get; init; } = string.Empty;
        public bool   Blocking { get; init; }
        public int    OrderSeq { get; init; }
        /// <summary>
        /// Event code fired automatically when this hook completes with success.
        /// Null if this hook is not a completing hook (no auto-advance).
        /// </summary>
        public int?   CompleteSuccessCode { get; init; }
        /// <summary>
        /// Event code fired automatically when this hook completes with failure.
        /// Null if no failure path is defined.
        /// </summary>
        public int?   CompleteFailureCode { get; init; }
        /// <summary>Parameter codes for this hook. Hook-own params take priority; falls back to parent rule params if empty.</summary>
        public IReadOnlyList<string> ParamCodes { get; init; } = System.Array.Empty<string>();

        public override string ToString() => $"{Route} | {Label} | order={OrderSeq} blocking={Blocking}{(CompleteSuccessCode.HasValue ? $" ok={CompleteSuccessCode}" : "")}{(CompleteFailureCode.HasValue ? $" fail={CompleteFailureCode}" : "")}";
    }
}
