using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleEvent {
        LifeCycleEventKind Kind { get; }
        long ConsumerId { get; }
        long InstanceId { get; }
        long DefinitionVersionId { get; }
        string ExternalRef { get; }
        string? RequestId { get; }
        DateTimeOffset OccurredAt { get; }
        string AckGuid { get; }
        bool AckRequired { get; }
        IReadOnlyDictionary<string, object?>? Payload { get; }
    }
}
