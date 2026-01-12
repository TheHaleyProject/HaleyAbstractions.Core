using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintManager {
        Task<DbRow> GetLatestDefVersionAsync(int envCode, string defName, CancellationToken ct = default);
        Task<DbRow> GetDefVersionByIdAsync(long defVersionId, CancellationToken ct = default);
        Task<LifeCycleBlueprint> GetBlueprintLatestAsync(int envCode, string defName, CancellationToken ct = default);
        Task<LifeCycleBlueprint> GetBlueprintByVersionIdAsync(long defVersionId, CancellationToken ct = default);
        void Clear();
        void Invalidate(int envCode, string defName);
        void Invalidate(long defVersionId);
        Task<int> EnsureConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
        Task<int> ResolveConsumerIdAsync(int envCode, string? consumerGuid, CancellationToken ct = default);
        Task<int> EnsureDefaultConsumerAsync(int envCode, CancellationToken ct = default);
        Task BeatConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
    }
}
