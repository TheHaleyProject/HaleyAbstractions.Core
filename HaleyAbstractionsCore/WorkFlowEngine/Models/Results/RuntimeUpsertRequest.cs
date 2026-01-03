using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class RuntimeUpsertRequest {
        public string InstanceGuid { get; set; }
        public long ActivityId { get; set; }
        public long StateId { get; set; }
        public string ActorId { get; set; }
        public long StatusId { get; set; }
        public long LcId { get; set; }
        public bool Frozen { get; set; }
        public object Data { get; set; }
        public object Payload { get; set; }
        public RuntimeUpsertRequest() { }
    }
}
