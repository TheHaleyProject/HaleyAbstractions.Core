namespace Haley.Models {
    public class ConsumerInboxFilter {
        /// <summary>Filter by WorkflowKind byte value (1=Transition, 2=Hook). Null = all.</summary>
        public byte? Kind { get; set; }
        public long? DefId { get; set; }
        public long? DefVersionId { get; set; }
        public string? EntityId { get; set; }
        public string? InstanceGuid { get; set; }
        public string? AckGuid { get; set; }
        public string? Route { get; set; }
        public int? EventCode { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 50;
    }
}
