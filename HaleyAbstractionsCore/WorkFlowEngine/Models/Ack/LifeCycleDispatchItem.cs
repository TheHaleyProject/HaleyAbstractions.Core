using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleDispatchItem : ILifeCycleDispatchItem {
        public LifeCycleDispatchKind Kind { get; set; }
        public long AckId { get; set; }
        public string AckGuid { get; set; }
        public long ConsumerId { get; set; }
        public int AckStatus { get; set; }
        public int RetryCount { get; set; }
        public DateTime LastRetryUtc { get; set; }
        public ILifeCycleEvent Event { get; set; }
    }
}
