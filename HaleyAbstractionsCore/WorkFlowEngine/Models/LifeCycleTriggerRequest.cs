using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTriggerRequest : LifeCycleInstanceKey {
        public string Event { get; set; }
        public string Actor { get; set; }
        public long? PolicyId { get; set; } //Optional.. User may decide to 
        public bool AckRequired { get; set; } = true;
        public LifeCycleInstanceFlag? Flag { get; set; } = null;
        public string? Metadata { get; set; }
        public IReadOnlyDictionary<string, object> Payload { get; set; }
        public DateTimeOffset? OccurredAt { get; set; }
        public bool SkipAckGate { get; set; } = false;
        public LifeCycleTriggerRequest() { }
    }
}
