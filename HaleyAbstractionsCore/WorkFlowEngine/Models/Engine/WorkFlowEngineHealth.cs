using System;

namespace Haley.Models {
    public sealed class WorkFlowEngineHealth {
        public DateTimeOffset UtcNow { get; set; }
        public bool IsMonitorRunning { get; set; }
        public TimeSpan MonitorInterval { get; set; }
        public int ConsumerTtlSeconds { get; set; }

        public int DueLifecyclePendingCount { get; set; }
        public int DueLifecycleDeliveredCount { get; set; }
        public int DueHookPendingCount { get; set; }
        public int DueHookDeliveredCount { get; set; }

        public int TotalConsumers { get; set; }
        public int AliveConsumers { get; set; }
        public int DownConsumers { get; set; }

        public int DefaultStateStaleCount { get; set; }
    }
}
