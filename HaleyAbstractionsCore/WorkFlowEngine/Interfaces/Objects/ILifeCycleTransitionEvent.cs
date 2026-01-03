using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleTransitionEvent : ILifeCycleEvent {
        long LifeCycleId { get; }
        long FromStateId { get; }
        long ToStateId { get; }
        long EventId { get; }
        int EventCode { get; }
        string EventName { get; }
        IReadOnlyDictionary<string, object?>? PrevStateMeta { get; }
        long? PolicyId { get; }
        string? PolicyHash { get; }
        string? PolicyJson { get; }
    }
}
