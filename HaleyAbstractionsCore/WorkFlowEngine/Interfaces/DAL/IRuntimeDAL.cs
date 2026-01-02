using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IRuntimeDAL {
        Task<DbRow?> GetByIdAsync(long runtimeId, DbExecutionLoad load = default);
        Task<DbRow?> GetByKeyAsync(long instanceId, long activityId, long stateId, string actorId, DbExecutionLoad load = default);
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, long activityId, long stateId, string actorId, long statusId, DbExecutionLoad load = default);
        Task<int> SetStatusAsync(long runtimeId, long statusId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, DbExecutionLoad load = default);
    }
}
