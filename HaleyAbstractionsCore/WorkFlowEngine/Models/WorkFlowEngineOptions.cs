using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class WorkFlowEngineOptions {
        public TimeSpan MonitorInterval { get; set; } = TimeSpan.FromMinutes(2);
        public int MonitorPageSize { get; set; } = 200;
        public TimeSpan AckPendingResendAfter { get; set; }= TimeSpan.FromSeconds(50);
        public TimeSpan AckDeliveredResendAfter { get; set; } = TimeSpan.FromMinutes(6);
        public int MaxRetryCount { get; set; } = 10; //Beyond which, the acknowledgement will be marked as failed and associated instance will be marked as suspended. //Here, application might be down (crashed) and the acknowledgement was not notified.. 
        public int ConsumerTtlSeconds { get; set; } = 120; //Consume should send heartbeats within this time window to be considered alive.
        public int ConsumerDownRecheckSeconds { get; set; } = 60;

        // Ack consumer resolution (fallbacks)
        public Func<LifeCycleConsumerType /* Consumer Type */, long? /*Definition Id*/, CancellationToken, Task<IReadOnlyList<long>>>? ResolveConsumers { get; set; } 

        // Optional overrides (if you want to inject your own concrete implementations)
        public IBlueprintManager? BlueprintManager { get; set; }
        public IBlueprintImporter? BlueprintImporter { get; set; }
        public IStateMachine? StateMachine { get; set; }
        public IPolicyEnforcer? PolicyEnforcer { get; set; }
        public IAckManager? AckManager { get; set; }
        public IRuntimeEngine? RuntimeEngine { get; set; }
    }
}
