using System;

namespace Haley.Models {
    public sealed class InstanceRefItem {
        public string ExternalRef { get; set; } = string.Empty;
        public string InstanceGuid { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
    }
}
