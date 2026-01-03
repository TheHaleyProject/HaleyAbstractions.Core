using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowEngine : ILifeCycleEngine, IAsyncDisposable {
        IStateMachine StateMachine { get; }
        IBlueprintManager? BlueprintManager { get; }
        IPolicyEnforcer? PolicyEnforcer { get; }
        IAckManager? AckManager { get; }
        IInstanceMonitor? InstanceMonitor { get; }
        IConsumerRegistry? ConsumerRegistry { get; }
        IWorkFlowDAL? Dal { get; }
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
