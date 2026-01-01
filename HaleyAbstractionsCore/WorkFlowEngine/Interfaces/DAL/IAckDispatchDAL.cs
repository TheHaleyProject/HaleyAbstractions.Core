using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckDispatchDAL {
        Task<DbRows> ListPendingLifecycleReadyPagedAsync(int ackStatus, DateTime olderThan, int skip, int take, CancellationToken ct = default);
        Task<DbRows> ListPendingHookReadyPagedAsync(int ackStatus, DateTime olderThan, int skip, int take, CancellationToken ct = default);
    }
}
