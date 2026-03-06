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
        public Task<int> RegisterEnvironmentAsync(int envCode, string? envDisplayName, CancellationToken ct = default);
    }
}
