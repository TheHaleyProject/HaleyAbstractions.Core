using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IInstanceDAL {
        Task<DbRow?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<DbRow?> GetByDefVersionAndExternalRefAsync(int defVersionId, string externalRef, CancellationToken ct = default);
        Task<long> UpsertByDefVersionAndExternalRefReturnIdAsync(int defVersionId, string externalRef, int currentStateId, int? lastEventId, int policyId, uint flags, CancellationToken ct = default);
        Task<int> UpdateCurrentStateCasAsync(long id, int expectedFromStateId, int newToStateId, int? lastEventId, CancellationToken ct = default);
        Task<int> AddFlagsAsync(long id, uint flags, CancellationToken ct = default);
        Task<int> RemoveFlagsAsync(long id, uint flags, CancellationToken ct = default);
    }
}
