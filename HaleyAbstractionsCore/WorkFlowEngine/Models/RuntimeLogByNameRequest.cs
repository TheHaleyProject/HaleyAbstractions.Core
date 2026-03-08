using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class RuntimeLogByNameRequest {
        /// <summary>Identifies the instance. Set InstanceGuid for fastest lookup, or EnvCode+DefName+EntityId for name-based resolution.</summary>
        public LifeCycleInstanceKey Instance { get; set; } = new();
        /// <summary>Activity name (e.g. "send-approval-email"). Normalized to lowercase + trimmed before storage.</summary>
        public string Activity { get; set; } = "";
        /// <summary>Status string (e.g. "running", "approved"). Normalized to lowercase + trimmed before storage.</summary>
        public string Status { get; set; } = "";
        public string ActorId { get; set; } = "";
        /// <summary>
        /// The AckGuid received in ILifeCycleHookEvent. When set, the engine anchors this runtime
        /// entry to the state that originally fired the hook — guaranteeing correctness during replay
        /// or fast-moving workflows where the instance may have already transitioned to a new state.
        /// If omitted, state is derived from the instance's current state (safe for non-replay usage).
        /// </summary>
        public string? AckGuid { get; set; }
        public object? Data { get; set; }
        public object? Payload { get; set; }
        public RuntimeLogByNameRequest() { }
    }
}
