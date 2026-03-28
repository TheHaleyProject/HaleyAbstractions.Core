namespace Haley.Models {
    public sealed class WorkFlowEngineSummary {
        public int EnvCode { get; set; }
        public long TotalInstances { get; set; }
        public long RunningInstances { get; set; }
        public long PendingAcks { get; set; }
    }
}
