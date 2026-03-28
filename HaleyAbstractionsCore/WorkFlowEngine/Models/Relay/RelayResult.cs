namespace Haley.Models {
    /// <summary>
    /// Result returned by WorkflowRelay.NextAsync.
    /// </summary>
    public sealed class RelayResult {
        public bool    Advanced  { get; init; }
        public string? NewState  { get; init; }
        /// <summary>"InvalidTransition", "BlockedByMonitor", "NoHandlerRegistered", etc.</summary>
        public string? Reason    { get; init; }

        public static RelayResult Ok(string newState)      => new() { Advanced = true,  NewState = newState };
        public static RelayResult Blocked(string reason)   => new() { Advanced = false, Reason   = reason  };
    }
}
