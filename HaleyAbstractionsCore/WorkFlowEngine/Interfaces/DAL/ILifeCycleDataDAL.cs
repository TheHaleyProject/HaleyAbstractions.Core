using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDataDAL {
        Task<DbRow?> GetByIdAsync(long lifeCycleId, DbExecutionLoad load = default);
        Task<int> UpsertAsync(long lifeCycleId, string? actor, string? payloadJson, DbExecutionLoad load = default);
    }
}
