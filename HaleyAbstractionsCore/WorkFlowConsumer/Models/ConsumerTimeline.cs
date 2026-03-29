using Haley.Enums;

namespace Haley.Models {

    public sealed class ConsumerTimeline {
        public long InstanceId { get; set; }
        public string InstanceGuid { get; set; } = string.Empty;
        public string EntityGuid { get; set; } = string.Empty;
        public string DefName { get; set; } = string.Empty;
        public int DefVersion { get; set; }
        /// <summary>Engine-side instance creation timestamp mirrored into the consumer timeline header.</summary>
        public DateTime Created { get; set; }
        public ConsumerTimelineInstance? Instance { get; set; }
        public IReadOnlyList<ConsumerTimelineItem> Items { get; set; } = Array.Empty<ConsumerTimelineItem>();
    }

    public sealed class ConsumerTimelineInstance {
        public long Id { get; set; }
        public string Guid { get; set; } = string.Empty;
        public string EntityGuid { get; set; } = string.Empty;
        public string DefName { get; set; } = string.Empty;
        public int DefVersion { get; set; }
        /// <summary>Engine-side instance creation timestamp mirrored into the consumer database.</summary>
        public DateTime Created { get; set; }
    }

    public sealed class ConsumerTimelineItem {
        public long InboxId { get; set; }
        public string AckGuid { get; set; } = string.Empty;
        /// <summary>WorkflowKind name — "Transition" or "Hook".</summary>
        public string Kind { get; set; } = string.Empty;
        public int? HandlerVersion { get; set; }
        public int? EventCode { get; set; }
        public string? Route { get; set; }
        /// <summary>Gate or Effect. Null for Transition rows.</summary>
        public HookType? HookType { get; set; }
        public int RunCount { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime Created { get; set; }
        public ConsumerTimelineStatus? InboxStatus { get; set; }
        public ConsumerTimelineOutbox? Outbox { get; set; }
        public IReadOnlyList<ConsumerTimelineAction> Actions { get; set; } = Array.Empty<ConsumerTimelineAction>();
    }

    public sealed class ConsumerTimelineStatus {
        /// <summary>InboxStatus name — "Received", "Processing", "Processed", "Failed".</summary>
        public string Status { get; set; } = string.Empty;
        public int AttemptCount { get; set; }
        public string? LastError { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime Modified { get; set; }
    }

    public sealed class ConsumerTimelineOutbox {
        /// <summary>AckOutcome name — "Processed", "Retry", "Failed".</summary>
        public string Outcome { get; set; } = string.Empty;
        /// <summary>OutboxStatus name — "Pending", "Sent", "Confirmed", "Failed".</summary>
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset? NextRetryAt { get; set; }
        public string? LastError { get; set; }
        public DateTime Modified { get; set; }
        public IReadOnlyList<ConsumerTimelineOutboxHistory> History { get; set; } = Array.Empty<ConsumerTimelineOutboxHistory>();
    }

    public sealed class ConsumerTimelineOutboxHistory {
        public int AttemptNo { get; set; }
        /// <summary>AckOutcome name.</summary>
        public string Outcome { get; set; } = string.Empty;
        /// <summary>OutboxStatus name.</summary>
        public string Status { get; set; } = string.Empty;
        public string? ResponsePayload { get; set; }
        public string? Error { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public sealed class ConsumerTimelineAction {
        public long ActionId { get; set; }
        public int ActionCode { get; set; }
        /// <summary>Per-delivery status from inbox_action: "Attempted", "Completed", "Failed".</summary>
        public string DeliveryStatus { get; set; } = string.Empty;
        public string? DeliveryError { get; set; }
        /// <summary>Instance-wide status from business_action: "Pending", "Running", "Completed", "Failed".</summary>
        public string BusinessStatus { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ResultJson { get; set; }
    }
}
