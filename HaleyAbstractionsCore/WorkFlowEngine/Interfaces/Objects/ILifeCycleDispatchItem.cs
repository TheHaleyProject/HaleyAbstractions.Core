using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDispatchItem {
        LifeCycleDispatchKind Kind { get; }
        long AckId { get; }
        string AckGuid { get; }
        long ConsumerId { get; }
        int AckStatus { get; }
        int RetryCount { get; }
        DateTime LastRetryUtc { get; }
        ILifeCycleEvent Event { get; }
    }
}
