using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IRuntimeEngine {
        Task<long> UpsertAsync(RuntimeUpsertRequest req, CancellationToken ct = default);
        Task<int> SetStatusAsync(long runtimeId, long statusId, CancellationToken ct = default);
        Task<int> SetFrozenAsync(long runtimeId, bool frozen, CancellationToken ct = default);
        Task<int> SetLcIdAsync(long runtimeId, long lcId, CancellationToken ct = default);
    }
}
