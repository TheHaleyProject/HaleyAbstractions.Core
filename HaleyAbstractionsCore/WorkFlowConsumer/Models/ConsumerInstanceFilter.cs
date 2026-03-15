namespace Haley.Models {
    public class ConsumerInstanceFilter {
        public string? EntityGuid { get; set; }
        public string? DefName { get; set; }
        /// <summary>Filter by instance.guid (the engine-assigned instance GUID).</summary>
        public string? Guid { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 50;
    }
}
