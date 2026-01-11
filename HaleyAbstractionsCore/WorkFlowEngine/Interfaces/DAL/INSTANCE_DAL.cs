using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IInstanceDAL {
        Task<DbRow?> GetByGuidAsync(string guid, DbExecutionLoad load = default);
        Task<long?> GetIdByGuidAsync(string guid, DbExecutionLoad load = default);

        Task<long?> GetIdByKeyAsync(long defVersionId, string externalRef, DbExecutionLoad load = default);
        Task<string?> UpsertByKeyReturnGuidAsync(long defVersionId, string externalRef, long currentStateId, long? lastEventId, long policyId, uint flags, DbExecutionLoad load = default);
        Task<int> UpdateCurrentStateCasAsync(long instanceId, long expectedFromStateId, long newToStateId, long? lastEventId, DbExecutionLoad load = default);

        Task<int> SetPolicyAsync(long instanceId, long policyId, DbExecutionLoad load = default);
        Task<int> AddFlagsAsync(long instanceId, uint flags, DbExecutionLoad load = default);
        Task<int> RemoveFlagsAsync(long instanceId, uint flags, DbExecutionLoad load = default);
    }

    public interface IHookDAL {
        Task<DbRow?> GetByIdAsync(long hookId, DbExecutionLoad load = default);
        Task<DbRow?> GetByKeyAsync(long instanceId, long stateId, long viaEventId, bool onEntry, string route, DbExecutionLoad load = default);
        Task<long> UpsertByKeyReturnIdAsync(long instanceId, long stateId, long viaEventId, bool onEntry, string route, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAndStateAsync(long instanceId, long stateId, DbExecutionLoad load = default);
        Task<int> DeleteAsync(long hookId, DbExecutionLoad load = default);
        Task<int> DeleteByInstanceAsync(long instanceId, DbExecutionLoad load = default);
    }

    public interface ILifeCycleDAL {
        Task<long> InsertAsync(long instanceId, long fromStateId, long toStateId, long eventId, DbExecutionLoad load = default);
        Task<DbRow?> GetLastByInstanceAsync(long instanceId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstanceAsync(long instanceId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstancePagedAsync(long instanceId, int skip, int take, DbExecutionLoad load = default);
        Task<string?> GetTimelineJsonByInstanceIdAsync(long instanceId, DbExecutionLoad load = default);
    }

    public interface ILifeCycleDataDAL {
        Task<DbRow?> GetByIdAsync(long lifeCycleId, DbExecutionLoad load = default);
        Task<int> UpsertAsync(long lifeCycleId, string? actor, string? payloadJson, DbExecutionLoad load = default);
        Task<int> DeleteAsync(long lifeCycleId, DbExecutionLoad load = default);
    }

    public interface ILifeCycleTimeoutDAL {
        Task<DbRows> ListDuePagedAsync(uint excludedInstanceFlagsMask, int skip, int take, DbExecutionLoad load = default);
        Task<int> InsertIgnoreAsync(long entryLcId, DbExecutionLoad load = default);
    }

}
