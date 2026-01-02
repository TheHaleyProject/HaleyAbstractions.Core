using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintReadDAL  {
        Task<DbRow?> EnvGetByCodeAsync(int code, CancellationToken ct = default);
        Task<DbRow?> DefGetByEnvAndNameAsync(int envId, string name, CancellationToken ct = default);
        Task<DbRow?> DefVersionGetLatestByDefAsync(int defId, CancellationToken ct = default);
        Task<DbRow?> DefVersionGetByIdAsync(int defVersionId, CancellationToken ct = default);
        Task<DbRows> StatesListByDefVersionAsync(int defVersionId, CancellationToken ct = default);
        Task<DbRows> EventsListByDefVersionAsync(int defVersionId, CancellationToken ct = default);
        Task<DbRows> TransitionsListByDefVersionAsync(int defVersionId, CancellationToken ct = default);
        Task<DbRows> PoliciesListByDefinitionAsync(int defId, CancellationToken ct = default);
        Task<DbRow?> PolicyGetByIdAsync(int policyId, CancellationToken ct = default);
    }
}
