using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public readonly struct DbExecutionLoad {
        public ITransactionHandler? Handler { get; }
        public CancellationToken Ct { get; }

        public DbExecutionLoad(CancellationToken ct, ITransactionHandler? th = null) {
            Ct = ct;
            Handler = th;
        }
        public static DbExecutionLoad None => default;
    }
}
