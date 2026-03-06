using Haley.Enums;

namespace Haley.Services {
    public sealed class ConsumerServiceOptions {
        /// <summary>Stable identity string for this consumer process. Used for engine registration and heartbeat.</summary>
        public string ConsumerGuid { get; set; } = string.Empty;
        /// <summary>Environment code this consumer operates in.</summary>
        public int EnvCode { get; set; }
        /// <summary>Max number of events dispatched concurrently per poll cycle.</summary>
        public int MaxConcurrency { get; set; } = 5;
        /// <summary>How many events to pull per poll cycle.</summary>
        public int BatchSize { get; set; } = 20;
        /// <summary>ACK status to query for due events (typically Pending = 1).</summary>
        public int AckStatus { get; set; } = 1;
        /// <summary>Consumer TTL window in seconds (engine-side liveness check).</summary>
        public int TtlSeconds { get; set; } = 120;
        /// <summary>Interval between heartbeat calls to the engine.</summary>
        public TimeSpan HeartbeatInterval { get; set; } = TimeSpan.FromSeconds(30);
        /// <summary>Delay between poll cycles when no events are found.</summary>
        public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(5);
        /// <summary>Delay between outbox retry cycles.</summary>
        public TimeSpan OutboxInterval { get; set; } = TimeSpan.FromSeconds(15);
        /// <summary>How long to wait before re-queuing a failed outbox ACK.</summary>
        public TimeSpan OutboxRetryDelay { get; set; } = TimeSpan.FromMinutes(2);
        /// <summary>Default handler upgrade policy for new instances.</summary>
        public HandlerUpgrade DefaultHandlerUpgrade { get; set; } = HandlerUpgrade.Pinned;
    }
}