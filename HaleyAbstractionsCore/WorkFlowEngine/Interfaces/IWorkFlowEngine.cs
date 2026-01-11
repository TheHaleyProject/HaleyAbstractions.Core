using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowEngine : ILifeCycleEngine, IAsyncDisposable {
        //IStateMachine StateMachine { get; }
        //IBlueprintManager BlueprintManager { get; }
        //IBlueprintImporter BlueprintImporter { get; }
        //IPolicyEnforcer PolicyEnforcer { get; }
        //IAckManager AckManager { get; }
        //IRuntimeEngine Runtime { get; }
        //ILifeCycleMonitor Monitor { get; }
        //IWorkFlowDAL Dal { get; }
    }
}
