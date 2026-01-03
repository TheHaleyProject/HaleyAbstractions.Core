using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleHookEvent : LifeCycleEvent, ILifeCycleHookEvent {
        public long HookId { get; set; }
        public long StateId { get; set; }
        public bool OnEntry { get; set; }
        public string HookCode { get; set; } = string.Empty;
        public string? OnSuccessEvent { get; set; }
        public string? OnFailureEvent { get; set; }
        public DateTimeOffset? NotBefore { get; set; }
        public DateTimeOffset? Deadline { get; set; }

        public LifeCycleHookEvent(long instanceId, string externalRef, DateTimeOffset occurredAt, string ackGuid)
            : base(instanceId, externalRef, occurredAt, ackGuid) {
            Kind = LifeCycleEventKind.Hook;
        }
    }
}