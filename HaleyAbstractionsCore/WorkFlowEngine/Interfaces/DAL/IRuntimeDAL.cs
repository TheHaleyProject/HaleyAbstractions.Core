using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IRuntimeDAL {
        Task<DbRow?> GetByIdAsync(long runtimeId, DbExecutionLoad load = default);
        Task<DbRow?> GetByKeyAsync(long instanceId, int activityId, int stateId, string actorId, DbExecutionLoad load = default);
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, int activityId, int stateId, string actorId, int statusId, DbExecutionLoad load = default);
        Task<int> SetStatusAsync(long runtimeId, int statusId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, DbExecutionLoad load = default);
    }
}
