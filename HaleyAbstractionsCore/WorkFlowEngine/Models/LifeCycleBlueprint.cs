using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleBlueprint {
        public long DefinitionVersionId { get; set; }
        public long DefinitionId { get; set; }
        public string Name { get; set; } = "";
        public int Version { get; set; }
    }
}
