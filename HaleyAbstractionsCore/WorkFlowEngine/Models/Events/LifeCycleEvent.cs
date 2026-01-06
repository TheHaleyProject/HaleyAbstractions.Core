using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public class LifeCycleEvent : ILifeCycleEvent {
        public LifeCycleEventKind Kind { get; }
        public long ConsumerId { get; set; }
        //public long InstanceId { get; set; } 
        public string InstanceGuid { get; set; }
        public long DefinitionVersionId { get; set; }
        public string ExternalRef { get; set; }
        public string AckGuid { get; set; }
        public string? RequestId { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public bool AckRequired { get; set; }
        public IReadOnlyDictionary<string, object?>? Payload { get; set; }
    }
}