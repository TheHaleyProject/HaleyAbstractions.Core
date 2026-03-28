namespace Haley.Models {
    public sealed class SnapshotHookRoute {
        public string Route    { get; init; } = string.Empty;
        public bool   Blocking { get; init; }
        public int    OrderSeq { get; init; }
    }
}
