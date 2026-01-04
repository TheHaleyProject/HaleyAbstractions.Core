using Haley.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class WorkFlowEngineOptions {
        public TimeSpan MonitorInterval { get; set; } = TimeSpan.FromMinutes(2);
        public int MonitorPageSize { get; set; } = 200;
        public int MonitorMaxTimeoutTriggersPerRun { get; set; } = 200;

        // Ack consumer resolution (fallbacks)
        public long DefaultConsumerId { get; set; } = 1857; //Year of sepoy mutiny
        public Func<long, long, IReadOnlyList<long>>? ResolveTransitionConsumers { get; set; } // (defVersionId, instanceId) => consumers
        public Func<long, long, string, IReadOnlyList<long>>? ResolveHookConsumers { get; set; } // (defVersionId, instanceId, hookCode) => consumers
        public IReadOnlyList<long>? MonitorConsumers { get; set; }

        // Optional overrides (if you want to inject your own concrete implementations)
        public IBlueprintManager? BlueprintManager { get; set; }
        public IBlueprintImporter? BlueprintImporter { get; set; }
        public IStateMachine? StateMachine { get; set; }
        public IPolicyEnforcer? PolicyEnforcer { get; set; }
        public IAckManager? AckManager { get; set; }
        public IRuntimeEngine? RuntimeEngine { get; set; }
    }
}
