using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IStateMachine {
        Task<DbRow> EnsureInstanceAsync(long defVersionId, string externalRef, DbExecutionLoad load = default);
        Task<ApplyTransitionResult> ApplyTransitionAsync(LifeCycleBlueprint bp, DbRow instance, string eventName, string? requestId, string? actor, IReadOnlyDictionary<string, object?>? payload, DbExecutionLoad load = default);
    }
}
