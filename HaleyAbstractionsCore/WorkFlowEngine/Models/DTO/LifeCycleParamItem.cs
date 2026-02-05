using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleParamItem {
        public string Code { get; set; } = string.Empty;
        public IReadOnlyDictionary<string, object?> Data { get; set; } = default!;
        public LifeCycleParamItem() { }
    }
}
