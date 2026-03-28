using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    // Internal model: WorkFlowEngine → RuntimeEngine. Consumers never use this directly.
    // Frozen and LcId removed — runtime entries are audit-only; the upsert itself handles updates.
    public sealed class RuntimeLogByIdRequest {
        public string InstanceGuid { get; set; }
        public long ActivityId { get; set; }
        public long StateId { get; set; }
        public string ActorId { get; set; }
        public long StatusId { get; set; }
        public object Data { get; set; }
        public object Payload { get; set; }
        public RuntimeLogByIdRequest() { }
    }
}
