using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleDispatchItem : ILifeCycleDispatchItem {
        public LifeCycleEventKind Kind { get; set; }
        public long AckId { get; set; }
        public string AckGuid { get; set; } = string.Empty;
        public long ConsumerId { get; set; }
        public int AckStatus { get; set; }
        public int TriggerCount { get; set; }
        public DateTime LastTrigger { get; set; }
        public DateTime? NextDue { get; set; }
        public ILifeCycleEvent Event { get; set; } = default!;
    }
}
