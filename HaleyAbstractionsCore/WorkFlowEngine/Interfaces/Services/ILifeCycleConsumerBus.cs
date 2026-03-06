using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleConsumerBus : IBlueprintImporter {
        Task AckAsync(long consumerId, string ackGuid, AckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, CancellationToken ct = default);
        Task AckAsync(int envCode, string consumerGuid, string ackGuid, AckOutcome outcome, string? message = null, DateTimeOffset? retryAt = null, CancellationToken ct = default);
        Task<long> RegisterConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
        Task BeatConsumerAsync(int envCode, string consumerGuid, CancellationToken ct = default);
        Task<int> RegisterEnvironmentAsync(int envCode, string? envDisplayName, CancellationToken ct = default);
        /// <summary>
        /// Resolves the engine-assigned definition ID for a given definition name and environment.
        /// Used by the consumer library at startup to bind auto-discovered wrappers to their def_ids.
        /// Returns null if no matching definition is found.
        /// </summary>
        Task<long?> GetDefinitionIdAsync(int envCode, string definitionName, CancellationToken ct = default);
    }
}
