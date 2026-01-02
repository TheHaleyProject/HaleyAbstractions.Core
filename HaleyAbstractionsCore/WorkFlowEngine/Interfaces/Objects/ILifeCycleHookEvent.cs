using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleHookEvent : ILifeCycleEvent {
        long HookId { get; }
        long StateId { get; }
        bool OnEntry { get; }
        string HookCode { get; }          // emit.event from policy json
        string? OnSuccessEvent { get; }   // emit.complete.success
        string? OnFailureEvent { get; }   // emit.complete.failure
        DateTimeOffset? NotBefore { get; }
        DateTimeOffset? Deadline { get; }
    }

}
