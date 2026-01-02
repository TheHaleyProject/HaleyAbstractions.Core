using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTriggerRequest {
        public int? EnvironmentCode { get; set; }
        public string? DefinitionName { get; set; }
        public long? DefinitionVersionId { get; set; }
        public string ExternalRef { get; set; } = "";
        public string EventName { get; set; } = "";          
        public string? RequestId { get; set; }
        public string? Actor { get; set; }
        public IReadOnlyDictionary<string, object?>? Payload { get; set; }
    }
}
