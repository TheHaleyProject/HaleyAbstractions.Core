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
        public bool Frozen { get; set; }
        public object? Data { get; set; }
        public object? Payload { get; set; }
        public RuntimeLogByNameRequest() { }
    }
}
