namespace Haley.Models {
    public class ConsumerInboxFilter {
        /// <summary>Filter by WorkflowKind byte value (1=Transition, 2=Hook). Null = all.</summary>
        public byte? Kind { get; set; }
        /// <summary>Filter by instance.guid (engine-assigned). Matched via JOIN with instance table.</summary>
        public string? InstanceGuid { get; set; }
        public string? AckGuid { get; set; }
        public string? Route { get; set; }
        public int? EventCode { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 50;
    }
}
