using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class StateDef {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public uint Flags { get; set; }
        public int? TimeoutMinutes { get; set; }
        public long? TimeoutEventId { get; set; }
        public bool IsInitial { get; set; }
        public StateDef() { }
    }
}
