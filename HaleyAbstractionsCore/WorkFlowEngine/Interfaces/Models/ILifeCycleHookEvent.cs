using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleHookEvent : ILifeCycleEvent {
        bool OnEntry { get; }
        string Route { get; }
        DateTimeOffset? NotBefore { get; }
        DateTimeOffset? Deadline { get; }
        bool IsBlocking { get; }
        string? GroupName { get; }
        int OrderSeq { get; }   // emission order stage for this hook
        int AckMode  { get; }   // 0=All; 1=Any
    }
}
