using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface ILifeCycleEngine : ILifeCycleConsumerBus,ILifeCycleRuntimeBus {
        Task RunMonitorOnceAsync(long consumerId, CancellationToken ct = default);
        Task StartMonitorAsync(CancellationToken ct = default);
        Task StopMonitorAsync(CancellationToken ct = default);
        Task<WorkFlowEngineHealth> GetHealthAsync(CancellationToken ct = default);
        Task<WorkFlowEngineSummary> GetSummaryAsync(int envCode, CancellationToken ct = default);
    }
}
