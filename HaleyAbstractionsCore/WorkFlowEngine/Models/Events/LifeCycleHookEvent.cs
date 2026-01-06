using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleHookEvent : LifeCycleEvent, ILifeCycleHookEvent {
        public override LifeCycleEventKind Kind { get { return LifeCycleEventKind.Hook; } }
        public long HookId { get; set; }
        public bool OnEntry { get; set; }
        public string HookCode { get; set; } //Route or identifier for the hook
        public string OnSuccessEvent { get; set; }
        public string OnFailureEvent { get; set; }
        public DateTimeOffset? NotBefore { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public static LifeCycleHookEvent Make(LifeCycleEvent evt) {
            if (evt is null) new LifeCycleHookEvent();
            return new LifeCycleHookEvent {
               ConsumerId = evt.ConsumerId,
               InstanceGuid = evt.InstanceGuid,
                DefinitionVersionId = evt.DefinitionVersionId,
                ExternalRef = evt.ExternalRef,
                AckGuid  = evt.AckGuid,
                RequestId = evt.RequestId,
                OccurredAt = evt.OccurredAt,
                AckRequired = evt.AckRequired,
                Payload =evt.Payload
            };
        }
    }
}