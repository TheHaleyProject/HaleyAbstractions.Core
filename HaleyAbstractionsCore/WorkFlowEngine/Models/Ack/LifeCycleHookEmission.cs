using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleHookEmission : ILifeCycleHookEmission {
        public long HookId { get; set; }
        public long StateId { get; set; }
        public bool OnEntry { get; set; }
        public string HookCode { get; set; }
        public string OnSuccessEvent { get; set; }
        public string OnFailureEvent { get; set; }
        public DateTimeOffset? NotBefore { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public IReadOnlyDictionary<string, object> Payload { get; set; }
    }
}
