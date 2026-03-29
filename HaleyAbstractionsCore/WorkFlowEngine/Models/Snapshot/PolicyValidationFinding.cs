namespace Haley.Models {
    public enum PolicyFindingSeverity { Warning, Error }

    /// <summary>
    /// A single finding from PolicyValidator — describes a specific issue in a policy rule's emit list.
    /// </summary>
    public sealed class PolicyValidationFinding {
        public PolicyFindingSeverity Severity   { get; init; }
        public string                State      { get; init; } = string.Empty;
        public int?                  Via        { get; init; }
        public string?               Route      { get; init; }
        public string                Code       { get; init; } = string.Empty;
        public string                Message    { get; init; } = string.Empty;

        public override string ToString() => $"[{Severity}] {State}{(Via.HasValue ? $" via {Via}" : "")} | {(Route != null ? Route + " | " : "")}{Code}: {Message}";
    }
}
