using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IInstanceDAL {
        Task<DbRow?> GetByIdAsync(long instanceId, DbExecutionLoad load = default);
        Task<DbRow?> GetByKeyAsync(long defVersionId, string externalRef, DbExecutionLoad load = default);

        Task<long> UpsertAsync(long defVersionId, string externalRef, long currentStateId, long? lastEventId, long policyId, uint flags, DbExecutionLoad load = default);

        Task<int> UpdateCurrentStateCasAsync(long instanceId, long expectedFromStateId, long newToStateId, long? lastEventId, DbExecutionLoad load = default);

        Task<int> SetPolicyAsync(long instanceId, long policyId, DbExecutionLoad load = default);
        Task<int> AddFlagsAsync(long instanceId, uint flags, DbExecutionLoad load = default);
        Task<int> RemoveFlagsAsync(long instanceId, uint flags, DbExecutionLoad load = default);
    }

}
