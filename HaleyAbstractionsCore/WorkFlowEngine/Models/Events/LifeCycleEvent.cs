using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public class LifeCycleEvent : ILifeCycleEvent {
        public LifeCycleEventKind Kind { get; protected set; }

        public long InstanceId { get; protected set; }
        public long DefinitionVersionId { get; private set; }   // optional
        public string ExternalRef { get; protected set; }

        public string? RequestId { get; private set; }
        public DateTimeOffset OccurredAt { get; protected set; }

        public string AckGuid { get; protected set; }           // changed (see below)
        public bool AckRequired { get; private set; }
        public IReadOnlyDictionary<string, object?>? Payload { get; private set; }

        public LifeCycleEvent(long instanceId, string externalRef, DateTimeOffset occurredAt, string ackGuid) {
            InstanceId = instanceId;
            ExternalRef = externalRef;
            OccurredAt = occurredAt;
            AckGuid = ackGuid;
        }

        // public, so engine can call it even from another library
        public void ApplyContext(
            long? defVersionId = null,
            string? requestId = null,
            bool? ackRequired = null,
            IReadOnlyDictionary<string, object?>? payload = null) {
            if (defVersionId.HasValue) DefinitionVersionId = defVersionId.Value;
            if (requestId != null) RequestId = requestId;
            if (ackRequired.HasValue) AckRequired = ackRequired.Value;
            if (payload != null) Payload = payload;
        }
    }

}