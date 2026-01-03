using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTriggerRequest {
        public int EnvCode { get; set; }
        public string DefName { get; set; }
        public string ExternalRef { get; set; }
        public string Event { get; set; }
        public string Actor { get; set; }
        public string RequestId { get; set; }
        public IReadOnlyDictionary<string, object> Payload { get; set; }
        public LifeCycleTriggerRequest() { }
    }
}
