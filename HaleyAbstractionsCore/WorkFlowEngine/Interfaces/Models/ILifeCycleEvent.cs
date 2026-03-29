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
        long DefinitionId { get; }
        long DefinitionVersionId { get; }
        string EntityId { get; }
        string AckGuid { get; }
        DateTimeOffset OccurredAt { get; }
        string? OnSuccessEvent { get; } // Used by transition and hook events; Complete events generally leave this null.
        string? OnFailureEvent { get; }
        string? Metadata { get; }
        IReadOnlyList<LifeCycleParamItem>? Params { get; } 
    }
}
