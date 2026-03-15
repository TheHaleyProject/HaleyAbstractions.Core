using Haley.Enums;

namespace Haley.Models {
    public sealed class ConsumerOutboxFilter : ConsumerInboxFilter {
        /// <summary>Filter by OutboxStatus byte value (1=Pending, 2=Confirmed, 3=Dead). Null = all.</summary>
        public byte? Status { get; set; }
        /// <summary>AckOutcome is defined in the engine abstractions — safe to use here.</summary>
        public AckOutcome? CurrentOutcome { get; set; }
    }
}
