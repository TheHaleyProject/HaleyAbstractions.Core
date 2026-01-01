using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleDataDAL {
        Task<DbRow?> GetByIdAsync(long lcId, CancellationToken ct = default);
        Task<int> UpsertAsync(long lcId, string? actor, string? payload, CancellationToken ct = default);
    }
}
