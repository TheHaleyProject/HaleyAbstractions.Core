using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckDAL {
        Task<DbRow?> GetByIdAsync(long ackId, DbExecutionLoad load = default);
        Task<DbRow?> GetByGuidAsync(string guid, DbExecutionLoad load = default);
        Task<long?> GetIdByGuidAsync(string guid, DbExecutionLoad load = default);

        Task<DbRow?> InsertReturnRowAsync(DbExecutionLoad load = default);
        Task<DbRow?> InsertWithGuidReturnRowAsync(string guid, DbExecutionLoad load = default);
        Task<int> DeleteAsync(long ackId, DbExecutionLoad load = default);
    }

    public interface IAckConsumerDAL {
        Task<DbRow?> GetByKeyAsync(long ackId, long consumer, DbExecutionLoad load = default);
        Task<DbRow?> GetByAckGuidAndConsumerAsync(string ackGuid, long consumer, DbExecutionLoad load = default);
        Task<long> UpsertByAckIdAndConsumerReturnIdAsync(long ackId, long consumer, int status, DbExecutionLoad load = default);
        Task<int> SetStatusAsync(long ackId, long consumer, int status, DbExecutionLoad load = default);
        Task<int> SetStatusByGuidAsync(string ackGuid, long consumer, int status, DbExecutionLoad load = default);
        Task<int> MarkRetryAsync(long ackId, long consumer, DbExecutionLoad load = default);
        Task<DbRows> ListByStatusPagedAsync(int status, int skip, int take, DbExecutionLoad load = default);
        Task<DbRows> ListByConsumerAndStatusPagedAsync(long consumer, int status, int skip, int take, DbExecutionLoad load = default);
        Task<DbRows> ListReadyForRetryAsync(int status, DateTime utcOlderThan, DbExecutionLoad load = default);
    }

    public interface IAckDispatchDAL {
        Task<DbRows> ListPendingLifecycleReadyPagedAsync(long consumer, int status, DateTime utcOlderThan, int skip, int take, DbExecutionLoad load = default);
        Task<DbRows> ListPendingHookReadyPagedAsync(long consumer, int status, DateTime utcOlderThan, int skip, int take, DbExecutionLoad load = default);
        Task<int?> CountPendingLifecycleReadyAsync(int status, DateTime utcOlderThan, DbExecutionLoad load = default);
        Task<int?> CountPendingHookReadyAsync(int status, DateTime utcOlderThan, DbExecutionLoad load = default);
    }

    public interface IHookAckDAL {
        Task<long?> GetAckIdByHookIdAsync(long hookId, DbExecutionLoad load = default);
        Task<int> AttachAsync(long ackId, long hookId, DbExecutionLoad load = default);
        Task<int> DeleteByHookIdAsync(long hookId, DbExecutionLoad load = default);
    }

    public interface ILcAckDAL {
        Task<long?> GetAckIdByLcIdAsync(long lcId, DbExecutionLoad load = default);
        Task<int> AttachAsync(long ackId, long lcId, DbExecutionLoad load = default);
        Task<int> DeleteByLcIdAsync(long lcId, DbExecutionLoad load = default);
    }
}
