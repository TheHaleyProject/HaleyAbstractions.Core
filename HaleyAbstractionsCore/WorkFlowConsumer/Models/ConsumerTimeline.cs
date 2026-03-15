namespace Haley.Models {

    public sealed class ConsumerTimeline {
        public string InstanceGuid { get; set; } = string.Empty;
        public IReadOnlyList<ConsumerTimelineItem> Items { get; set; } = Array.Empty<ConsumerTimelineItem>();
    }

    public sealed class ConsumerTimelineItem {
        public long InboxId { get; set; }
        public string AckGuid { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        /// <summary>WorkflowKind name — "Transition" or "Hook".</summary>
        public string Kind { get; set; } = string.Empty;
        public long DefId { get; set; }
        public long DefVersionId { get; set; }
        public int? HandlerVersion { get; set; }
        public int? EventCode { get; set; }
        public string? Route { get; set; }
        public int RunCount { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime Created { get; set; }
        public ConsumerTimelineStatus? InboxStatus { get; set; }
        public ConsumerTimelineOutbox? Outbox { get; set; }
        public IReadOnlyList<ConsumerTimelineStep> Steps { get; set; } = Array.Empty<ConsumerTimelineStep>();
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
        /// <summary>OutboxStatus name — "Pending", "Confirmed", "Dead".</summary>
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

    public sealed class ConsumerTimelineStep {
        public int StepCode { get; set; }
        /// <summary>InboxStepStatus name — "Running", "Completed", "Failed".</summary>
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ResultJson { get; set; }
        public string? LastError { get; set; }
    }
}
