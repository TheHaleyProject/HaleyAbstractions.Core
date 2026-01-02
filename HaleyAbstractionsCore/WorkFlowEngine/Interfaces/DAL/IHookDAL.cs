using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IHookDAL {
        Task<DbRow?> GetByIdAsync(long hookId, DbExecutionLoad load = default);
        Task<DbRow?> GetByKeyAsync(long instanceId, long stateId, long? viaEventId, bool onEntry, string hookCode, DbExecutionLoad load = default);
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, long stateId, long? viaEventId, bool onEntry, string hookCode, string? payloadJson, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, DbExecutionLoad load = default);
        Task<int> DeleteByInstanceAsync(long instanceId, DbExecutionLoad load = default);
    }
}
