using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDAL {
        Task<long> InsertAsync(long instanceId, long fromStateId, long toStateId, long eventId, DbExecutionLoad load = default);

        Task<DbRow?> GetLastByInstanceAsync(long instanceId, DbExecutionLoad load = default);
        Task<DbRows> ListByInstancePagedAsync(long instanceId, int skip, int take, DbExecutionLoad load = default);
    }

    
}
