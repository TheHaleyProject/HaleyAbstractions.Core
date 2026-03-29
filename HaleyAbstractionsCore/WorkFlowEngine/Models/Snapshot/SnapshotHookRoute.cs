using Haley.Enums;

namespace Haley.Models {
    public sealed class SnapshotHookRoute {
        public string   Route   { get; init; } = string.Empty;
        public string   Label   { get; init; } = string.Empty;
        public HookType Type    { get; init; } = HookType.Gate;
        public int      OrderSeq { get; init; }
        /// <summary>
        /// Raw send directive from policy JSON.
        /// Null means the field was not provided and the default "no" behavior applies.
        /// Validation decides whether the supplied value is acceptable.
        /// </summary>
        public string? SendModeRaw { get; init; }
        /// <summary>
        /// Effect-only carry-forward flag for success paths.
        /// False means the effect runs only if its own order is naturally reached.
        /// True means the effect may still run after an earlier gate success short-circuits later gate orders.
        /// Ignored for gate hooks.
        /// </summary>
        public bool SendAlways => Type == HookType.Effect && string.Equals(SendModeRaw, "always", System.StringComparison.OrdinalIgnoreCase);
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

        public override string ToString() => $"{Route} | {Label} | order={OrderSeq} type={Type}{(!string.IsNullOrWhiteSpace(SendModeRaw) ? $" send={SendModeRaw}" : "")}{(CompleteSuccessCode.HasValue ? $" ok={CompleteSuccessCode}" : "")}{(CompleteFailureCode.HasValue ? $" fail={CompleteFailureCode}" : "")}";
    }
}
