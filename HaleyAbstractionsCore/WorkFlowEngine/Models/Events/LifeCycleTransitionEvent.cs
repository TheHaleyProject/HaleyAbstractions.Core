using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public class LifeCycleTransitionEvent : LifeCycleEvent, ILifeCycleTransitionEvent {
        public long LifeCycleId { get; set; }
        public long FromStateId { get; set; }
        public long ToStateId { get; set; }
        public long EventId { get; set; }
        public int EventCode { get; set; }
        public string EventName { get; set; } = string.Empty;

        public IReadOnlyDictionary<string, object?>? PrevStateMeta { get; set; }

        public long? PolicyId { get; set; }
        public string? PolicyHash { get; set; }
        public string? PolicyJson { get; set; }

        public LifeCycleTransitionEvent(long instanceId, string externalRef, DateTimeOffset occurredAt, string ackGuid)
            : base(instanceId, externalRef, occurredAt, ackGuid) {
            Kind = LifeCycleEventKind.Transition;
        }
    }
}