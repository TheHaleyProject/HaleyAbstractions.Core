using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTransitionEvent : LifeCycleEvent, ILifeCycleTransitionEvent {
        public override LifeCycleEventKind Kind { get { return LifeCycleEventKind.Transition; } }
        public long LifeCycleId { get; set; }
        public long FromStateId { get; set; }
        public long ToStateId { get; set; }
        public long EventId { get; set; }
        public int EventCode { get; set; }
        public string EventName { get; set; }
        public IReadOnlyDictionary<string, object> PrevStateMeta { get; set; }
        public long? PolicyId { get; set; }
        public string PolicyHash { get; set; }
        public string PolicyJson { get; set; }
    }
}