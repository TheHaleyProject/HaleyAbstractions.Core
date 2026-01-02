using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public readonly struct DbArg {
        public string Name { get; }
        public object? Value { get; }

        public DbArg(string name, object? value) { Name = name; Value = value; }

        public void Deconstruct(out string name, out object? value) { name = Name; value = Value; }

        public static implicit operator DbArg((string Name, object? Value) t) => new DbArg(t.Name, t.Value);
    }
}
