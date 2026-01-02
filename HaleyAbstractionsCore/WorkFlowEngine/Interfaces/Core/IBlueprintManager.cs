using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintManager {
        Task<DbRow> GetLatestDefVersionAsync(int envCode, string defName, CancellationToken ct = default);
        Task<DbRow> GetDefVersionByIdAsync(long defVersionId, CancellationToken ct = default);

        // returns compiled blueprint (internal model is fine)
        Task<LifeCycleBlueprint> GetBlueprintLatestAsync(int envCode, string defName, CancellationToken ct = default);
        Task<LifeCycleBlueprint> GetBlueprintByVersionIdAsync(long defVersionId, CancellationToken ct = default);

        void Clear();
        void Invalidate(int envCode, string defName);
        void Invalidate(long defVersionId);
    }
}
