using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDispatchItem {
        LifeCycleEventKind Kind { get; }
        long AckId { get; }
        string AckGuid { get; }
        long ConsumerId { get; }
        int AckStatus { get; }
        int TriggerCount { get; }
        DateTime LastTrigger { get; }
        DateTime? NextDue { get; }
        ILifeCycleEvent Event { get; }
    }
}
