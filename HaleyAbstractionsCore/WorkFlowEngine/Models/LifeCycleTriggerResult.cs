using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTriggerResult {
        public bool Status { get; set; }
        public bool Transitioned { get; set; }
        public long InstanceId { get; set; }
        public long? LifeCycleId { get; set; }
        public string? Notice { get; set; }
    }
}
