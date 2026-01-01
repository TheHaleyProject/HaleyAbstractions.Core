using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IRuntimeDAL {
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, int activityId, int stateId, string actorId, int statusId, CancellationToken ct = default);
        Task<int> SetStatusAsync(long runtimeId, int statusId, CancellationToken ct = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, CancellationToken ct = default);
    }
}
