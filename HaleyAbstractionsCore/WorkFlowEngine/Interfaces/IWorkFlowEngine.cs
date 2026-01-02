using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowEngine : ILifeCycleEngine, IAsyncDisposable {
        IStateMachine StateMachine { get; }                 // MUST exist (minimum engine)
        IBlueprintManager? BlueprintManager { get; }        // optional
        IPolicyEnforcer? PolicyEnforcer { get; }            // optional
        IAckManager? AckManager { get; }                    // optional
        IInstanceMonitor? InstanceMonitor { get; }          // optional
        IWorkFlowDAL? Dal { get; }                         
        Task StartAsync(CancellationToken ct = default);     
        Task StopAsync(CancellationToken ct = default);     
    }
}
