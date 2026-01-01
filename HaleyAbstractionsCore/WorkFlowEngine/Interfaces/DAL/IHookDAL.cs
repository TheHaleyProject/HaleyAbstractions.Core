using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IHookDAL {
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, int stateId, int viaEventId, bool onEntry, string route, CancellationToken ct = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, CancellationToken ct = default);
        Task<int> DeleteByInstanceAsync(long instanceId, CancellationToken ct = default);
    }
}
