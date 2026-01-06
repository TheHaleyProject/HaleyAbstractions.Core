using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleTransitionEvent : LifeCycleEvent, ILifeCycleTransitionEvent {
        public LifeCycleEventKind Kind { get { return LifeCycleEventKind.Transition; } }
        public long LifeCycleId { get; set; }
        public long FromStateId { get; set; }
        public long ToStateId { get; set; }
        public int EventCode { get; set; }
        public string EventName { get; set; }
        public IReadOnlyDictionary<string, object> PrevStateMeta { get; set; }
        public string PolicyJson { get; set; }
        public static LifeCycleTransitionEvent Make(LifeCycleEvent evt) {
            if (evt is null) new LifeCycleTransitionEvent();
            return new LifeCycleTransitionEvent {
                ConsumerId = evt.ConsumerId,
                InstanceGuid = evt.InstanceGuid,
                DefinitionVersionId = evt.DefinitionVersionId,
                ExternalRef = evt.ExternalRef,
                AckGuid = evt.AckGuid,
                RequestId = evt.RequestId,
                OccurredAt = evt.OccurredAt,
                AckRequired = evt.AckRequired,
                Payload = evt.Payload
            };
        }
    }
}