namespace Haley.Models {
    public sealed class ConsumerWorkflowFilter {
        public string? EntityId { get; set; }
        public string? DefName { get; set; }
        public string? InstanceId { get; set; }
        public bool? IsTriggered { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 50;
    }
}
