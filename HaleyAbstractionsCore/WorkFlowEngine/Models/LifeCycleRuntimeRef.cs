using System;

namespace Haley.Models {
    /// <summary>
    /// Business-key reference to a specific runtime log entry.
    /// Pass Id (returned by UpsertRuntimeAsync) for a direct row lookup — fastest and most reliable.
    /// Fall back to Instance + Activity + ActorId for key-based resolution when Id is unavailable.
    /// </summary>
    public class LifeCycleRuntimeRef {
        /// <summary>Identifies the instance (by InstanceGuid OR EnvCode+DefName+EntityId).</summary>
        public LifeCycleInstanceKey Instance { get; set; } = new();
        /// <summary>Activity name (e.g. "send-approval-email"). Same value passed to UpsertRuntimeAsync.</summary>
        public string Activity { get; set; } = "";
        /// <summary>Actor / consumer identifier that owns this runtime entry.</summary>
        public string ActorId { get; set; } = "";
        /// <summary>Runtime row id returned by UpsertRuntimeAsync. When set, key-based lookup is skipped entirely.</summary>
        public long? Id { get; set; }
        public LifeCycleRuntimeRef() { }
    }
}
