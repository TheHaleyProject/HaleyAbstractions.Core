using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleEvent {
        LifeCycleEventKind Kind { get; }
        long ConsumerId { get; }
        string InstanceGuid { get; }
        string ExternalRef { get; }
        string? RequestId { get; }
        string AckGuid { get; }
        DateTimeOffset OccurredAt { get; }
        bool AckRequired { get; }
        IReadOnlyDictionary<string, object?>? Payload { get; }
    }
}
