using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDAL {
        Task<long> InsertAsync(long instanceId, int fromStateId, int toStateId, int eventId, CancellationToken ct = default);
        Task<DbRow?> GetLastByInstanceAsync(long instanceId, CancellationToken ct = default);
        Task<DbRows> ListByInstancePagedAsync(long instanceId, int skip, int take, CancellationToken ct = default);
    }
}
