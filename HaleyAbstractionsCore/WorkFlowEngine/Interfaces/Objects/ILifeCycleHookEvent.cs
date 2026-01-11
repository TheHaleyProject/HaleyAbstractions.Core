using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleHookEvent : ILifeCycleEvent {
        long HookId { get; }
        bool OnEntry { get; }
        string HookCode { get; }
        string? OnSuccessEvent { get; }
        string? OnFailureEvent { get; }
        DateTimeOffset? NotBefore { get; }
        DateTimeOffset? Deadline { get; }
    }
}
