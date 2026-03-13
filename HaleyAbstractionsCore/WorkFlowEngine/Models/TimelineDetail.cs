namespace Haley.Enums {
    /// <summary>
    /// Controls how much detail is returned in GetTimelineJsonAsync / GetTimelineHtmlAsync.
    /// </summary>
    public enum TimelineDetail {
        /// <summary>High-level transitions only (from/to state, event, timestamp). Safe for external consumers.</summary>
        Minimal = 0,
        /// <summary>Transitions + handler activities (who ran, status, duration). Default for managers.</summary>
        Detailed  = 1,
        /// <summary>Transitions + activities + emit/hook telemetry (dispatch count, ACK status, retries). For administrators.</summary>
        Admin    = 2
    }
}
