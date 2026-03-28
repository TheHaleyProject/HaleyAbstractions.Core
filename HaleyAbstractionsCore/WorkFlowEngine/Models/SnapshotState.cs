namespace Haley.Models {
    public sealed class SnapshotState {
        public string Name       { get; init; } = string.Empty;
        public bool   IsInitial  { get; init; }
        public bool   IsTerminal { get; init; }
    }
}
