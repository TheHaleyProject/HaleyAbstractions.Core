namespace Haley.Models {
    public sealed class ConsumerInboxStatusFilter : ConsumerInboxFilter {
        /// <summary>Filter by InboxStatus byte value (1=Received, 2=Processing, 3=Processed, 4=Failed). Null = all.</summary>
        public byte? Status { get; set; }
    }
}
