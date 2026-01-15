using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class RuntimeLogByNameRequest {
        public string InstanceGuid { get; set; } = "";
        public long StateId { get; set; }
        public string Activity { get; set; } = "";     
        public string Status { get; set; } = "";      
        public string ActorId { get; set; } = "";      // consumer provides (varchar)
        public long LcId { get; set; }               
        public bool Frozen { get; set; }              
        public object? Data { get; set; }
        public object? Payload { get; set; }
        public RuntimeLogByNameRequest() { }
    }
}
