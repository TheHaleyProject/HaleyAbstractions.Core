using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IPolicyEnforcer {
        // policy to include in TRANSITION event (full json)
        Task<PolicyResolution> ResolvePolicyAsync(LifeCycleBlueprint bp, DbRow instance, ApplyTransitionResult applied, DbExecutionLoad load = default);
        // create hook rows + ack rows and return hook events to publish
        Task<IReadOnlyList<ILifeCycleHookEvent>> EmitHooksAsync(LifeCycleBlueprint bp, DbRow instance, ApplyTransitionResult applied, DbExecutionLoad load = default);
    }
}
