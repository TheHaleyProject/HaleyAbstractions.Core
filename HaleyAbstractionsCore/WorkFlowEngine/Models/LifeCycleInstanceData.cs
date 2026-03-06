using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleInstanceData {
        public long InstanceId { get; set; }
        public string InstanceGuid { get; set; }
        public long DefinitionId { get; set; }
        public long DefinitionVersionId { get; set; }
        public string EntityId { get; set; }
        public long CurrentStateId { get; set; }
        public string? Metadata { get; set; }
        public string? Context { get; set; }
    }
}
