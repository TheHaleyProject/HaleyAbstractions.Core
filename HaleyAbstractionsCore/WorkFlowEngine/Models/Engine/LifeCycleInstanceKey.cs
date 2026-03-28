using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class LifeCycleInstanceKey {
        public int EnvCode { get; set; }
        public string? DefName { get; set; }
        public string? EntityId { get; set; } //Entity Id alone cannot be used.. Because same entity might go through mulitple definition.. may be also across multiple environments.. say, we have an entity, which is distributed.. it can be controlled by different consumers running across different environemtns.. for us, environement is not by dev/prod/ etc.. it is by a logical group.
        public string? InstanceGuid { get; set; } //If this is present, it gets high priority, we can directy fetch the instance.
        public LifeCycleInstanceKey() { }
    }
}
