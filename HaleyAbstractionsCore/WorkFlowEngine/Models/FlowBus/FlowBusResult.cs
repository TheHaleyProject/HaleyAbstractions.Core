namespace Haley.Models {
    /// <summary>
    /// Result returned by IFlowBus.NextAsync.
    /// FlowBus never swallows failures — Applied=false with a Reason when blocked.
    /// </summary>
    public sealed class FlowBusResult {
        public bool    Applied { get; init; }
        public string? Reason  { get; init; }

        public static FlowBusResult Ok()                   => new() { Applied = true };
        public static FlowBusResult Blocked(string reason) => new() { Applied = false, Reason = reason };
    }
}
