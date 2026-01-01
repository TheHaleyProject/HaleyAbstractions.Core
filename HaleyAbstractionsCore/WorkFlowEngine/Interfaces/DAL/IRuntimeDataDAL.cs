using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IRuntimeDataDAL {
        Task<DbRow?> GetByIdAsync(long runtimeId, CancellationToken ct = default);
        Task<int> UpsertAsync(long runtimeId, string? data, string? payload, CancellationToken ct = default);
    }
}
