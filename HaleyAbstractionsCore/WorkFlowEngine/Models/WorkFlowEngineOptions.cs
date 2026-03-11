using Haley.Abstractions;
using Haley.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    //Dont mark as sealed.. this needs to be inherited by other classes.
    public class WorkFlowEngineOptions {
        [ConfigurationKeyName("monitor_interval")]
        public TimeSpan MonitorInterval { get; set; } = TimeSpan.FromMinutes(2);
        [ConfigurationKeyName("monitor_page_size")]
        public int MonitorPageSize { get; set; } = 200;
        [ConfigurationKeyName("pending_resend")]
        public TimeSpan AckPendingResendAfter { get; set; }= TimeSpan.FromSeconds(50);
        [ConfigurationKeyName("delivered_resend")]
        public TimeSpan AckDeliveredResendAfter { get; set; } = TimeSpan.FromMinutes(6);
        [ConfigurationKeyName("stale_timeout")]
        public TimeSpan DefaultStateStaleDuration { get; set; } = TimeSpan.FromMinutes(5);
        [ConfigurationKeyName("max_retry")]
        public int MaxRetryCount { get; set; } = 10; //Beyond which, the acknowledgement will be marked as failed and associated instance will be marked as suspended. //Here, application might be down (crashed) and the acknowledgement was not notified.. 
        [ConfigurationKeyName("consumer_ttl")]
        public int ConsumerTtlSeconds { get; set; } = 120; //Consume should send heartbeats within this time window to be considered alive.
        [ConfigurationKeyName("consumer_recheck")]
        public int ConsumerDownRecheckSeconds { get; set; } = 60;
        // When true, TriggerAsync will return Applied=false (Reason="BlockedByPendingAck") if the last lifecycle
        // entry for this instance still has unresolved ack_consumer rows (status Pending or Delivered).
        // Per-request override: set LifeCycleTriggerRequest.SkipAckGate=true to bypass.
        [ConfigurationKeyName("ack_gate")]
        public bool AckGateEnabled { get; set; } = false;

        // Ack consumer resolution (fallbacks)
        public Func<LifeCycleConsumerType /* Consumer Type */, long? /*Definition Id*/, CancellationToken, Task<IReadOnlyList<long>>>? ResolveConsumers { get; set; }

        //public TargetDB Dbtype { get; set; } = TargetDB.maria; //Let us start with Mariadb.. Currently we dont have implementation for any other database type.
    }
}
