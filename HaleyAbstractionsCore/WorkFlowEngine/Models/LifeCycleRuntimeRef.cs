using System;

namespace Haley.Models {
    /// <summary>
    /// Business-key reference to a specific runtime log entry.
    /// Use in SetRuntimeStatusAsync / FreezeRuntimeAsync / UnfreezeRuntimeAsync
    /// instead of storing the raw runtimeId returned by UpsertRuntimeAsync.
    /// </summary>
    public class LifeCycleRuntimeRef {
        /// <summary>Identifies the instance (by InstanceGuid OR EnvCode+DefName+EntityId).</summary>
        public LifeCycleInstanceKey Instance { get; set; } = new();
        /// <summary>Activity display name (e.g. "SendApprovalEmail").</summary>
        public string Activity { get; set; } = "";
        /// <summary>Actor / consumer identifier that owns this runtime entry.</summary>
        public string ActorId { get; set; } = "";
        /// <summary>State DB id — obtained from ILifeCycleEvent or ILifeCycleTransitionEvent.</summary>
        public long StateId { get; set; }
        public long? Id { get; set; } //Runtime Id, incase this is present, give first priority.
        public LifeCycleRuntimeRef() { }
    }
}
