using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTriggerResult {
        public bool Applied { get; set; }
        public string InstanceGuid { get; set; }
        public long InstanceId { get; set; }
        public long? LifeCycleId { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string Reason { get; set; }
        public IReadOnlyList<string> LifecycleAckGuids { get; set; }
        public IReadOnlyList<string> HookAckGuids { get; set; }
        public LifeCycleTriggerResult() { }
    }
}
