namespace Haley.Models {
    /// <summary>
    /// Centralised error/warning codes for the HaleyFlow Protocol.
    ///
    /// Ranges:
    ///   HFD1xxx — Definition structural errors and warnings (PolicyValidator, structural checks)
    ///   HFP2xxx — Policy hook/rule errors and warnings   (PolicyValidator, hook checks)
    ///   HFR3xxx — Relay runtime blocked conditions       (WorkflowRelay.NextAsync)
    ///   HFB4xxx — Backfill validation                    (WorkflowBackfillValidator)
    ///   HFF5xxx — FlowBus / relay host routing errors    (FlowBus, WorkflowRelayHost)
    /// </summary>
    public static class HaleyFlowErrorCodes {

        // ── HFD1xxx — Definition structural ─────────────────────────────────────

        /// <summary>No state is marked as initial. The relay cannot determine the start point.</summary>
        public const string NoInitialState         = "HFD1001";

        /// <summary>More than one state is marked as initial. Only one is allowed.</summary>
        public const string MultipleInitialStates  = "HFD1002";

        /// <summary>Transition references a FromState that does not exist in the states list.</summary>
        public const string UnknownFromState       = "HFD1003";

        /// <summary>Transition references a ToState that does not exist in the states list.</summary>
        public const string UnknownToState         = "HFD1004";

        /// <summary>The same event code is used more than once from the same state — routing is ambiguous.</summary>
        public const string DuplicateEventCode     = "HFD1005";

        /// <summary>A terminal state has outgoing transitions — a terminal state cannot be left.</summary>
        public const string TerminalHasTransitions = "HFD1006";

        /// <summary>A non-terminal state has no outgoing transitions — the workflow will get permanently stuck.</summary>
        public const string StuckState             = "HFD1007";

        /// <summary>A state's only outgoing transition is a self-loop — infinite loop guaranteed, no way out.</summary>
        public const string CircularOnly           = "HFD1008";

        /// <summary>State has no incoming transitions and is not the initial state — it can never be reached.</summary>
        public const string UnreachableState       = "HFD1101";   // Warning

        /// <summary>State has a self-loop but also has exit transitions — likely intentional but flagged for review.</summary>
        public const string SelfLoopWithExit       = "HFD1102";   // Warning

        // ── HFP2xxx — Policy hook/rule ───────────────────────────────────────────

        /// <summary>Policy rule targets a state not in the definition — dead rule, will never fire.</summary>
        public const string PolicyUnknownState     = "HFP2001";   // Warning

        /// <summary>Multiple blocking hooks at the same order define complete codes — execution order is undefined.</summary>
        public const string AmbiguousOrder         = "HFP2002";   // Error

        /// <summary>Non-blocking hook defines complete codes — they are silently ignored at runtime.</summary>
        public const string NonBlockingWithComplete = "HFP2101";  // Warning

        /// <summary>Hook is unreachable — a prior blocking hook with both success+failure codes terminates all paths.</summary>
        public const string UnreachableHook        = "HFP2102";   // Warning

        /// <summary>Blocking hook has no failure code and the transition has no failure code — system cannot auto-advance on failure; code-side Tier 2/3 must decide.</summary>
        public const string NoFailurePath          = "HFP2103";   // Warning

        // ── HFR3xxx — Relay runtime ──────────────────────────────────────────────

        /// <summary>Relay reached NextAsync but the definition has no initial state to resolve from.</summary>
        public const string RelayNoInitialState    = "HFR3001";

        /// <summary>No transition exists from the current state via the supplied event code.</summary>
        public const string RelayInvalidTransition = "HFR3002";

        /// <summary>Monitor callback returned false — execution was blocked before the handler ran.</summary>
        public const string RelayBlockedByMonitor  = "HFR3003";

        /// <summary>Transition handler returned false — hooks were skipped; no failure path was available to auto-advance.</summary>
        public const string RelayTransitionFailed  = "HFR3004";

        /// <summary>A blocking hook returned false and no failure path was available — state rolled back to FromState.</summary>
        public const string RelayBlockingHookFailed = "HFR3005";

        // ── HFB4xxx — Backfill validation ────────────────────────────────────────

        /// <summary>WorkflowName is missing on the backfill object.</summary>
        public const string BackfillMissingWorkflowName = "HFB4001";

        /// <summary>EntityRef is missing on the backfill object.</summary>
        public const string BackfillMissingEntityRef    = "HFB4002";

        /// <summary>The backfill object contains no transitions.</summary>
        public const string BackfillNoTransitions       = "HFB4003";

        /// <summary>The workflow definition was not found in the engine for the given name and env code.</summary>
        public const string BackfillDefinitionNotFound  = "HFB4004";

        /// <summary>A transition in the backfill object (FromState → ToState via EventCode) is not valid in the definition.</summary>
        public const string BackfillInvalidTransition   = "HFB4005";

        /// <summary>A hook entry in the backfill object has an empty Route — it will be skipped.</summary>
        public const string BackfillEmptyHookRoute      = "HFB4101";  // Warning

        /// <summary>A hook route in the backfill object is not defined in the policy for this transition — recorded as-is.</summary>
        public const string BackfillUnknownHookRoute    = "HFB4102";  // Warning

        // ── HFF5xxx — FlowBus / relay host ───────────────────────────────────────

        /// <summary>No workflow executor is registered — neither IWorkFlowConsumerService (engine) nor IWorkflowRelayService (relay).</summary>
        public const string FlowBusNoExecutor          = "HFF5001";

        /// <summary>Engine mode was requested but IWorkFlowConsumerService is not registered.</summary>
        public const string FlowBusEngineNotRegistered = "HFF5002";

        /// <summary>Engine mode requires a StartEvent but none was provided.</summary>
        public const string FlowBusMissingStartEvent   = "HFF5003";

        /// <summary>Engine rejected the initiation request.</summary>
        public const string FlowBusEngineRejected      = "HFF5004";

        /// <summary>Relay mode was requested but IWorkflowRelayService is not registered.</summary>
        public const string FlowBusRelayNotRegistered  = "HFF5005";

        /// <summary>No relay is registered for the requested workflow name.</summary>
        public const string RelayHostNoRelay           = "HFF5006";

        /// <summary>The StartEvent value is not a valid integer event code for relay mode.</summary>
        public const string RelayHostInvalidStartEvent = "HFF5007";
    }
}
