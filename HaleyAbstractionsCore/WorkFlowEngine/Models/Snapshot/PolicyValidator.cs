using System.Collections.Generic;
using System.Linq;

namespace Haley.Models {
    /// <summary>
    /// Validates a WorkflowDefinitionSnapshot — both structural (transition/state) and policy (hook) rules.
    /// Generic — used by both the relay (at Initialize time) and the engine (at definition import time).
    ///
    /// Critical errors (HasCriticalErrors = true) must reject registration.
    /// Warnings are informational — allowed but flagged.
    /// </summary>
    public static class PolicyValidator {
        public static PolicyValidationResult Validate(WorkflowDefinitionSnapshot snapshot) {
            var findings = new List<PolicyValidationFinding>();

            ValidateStructure(snapshot, findings);
            ValidatePolicy(snapshot, findings);

            return new PolicyValidationResult { Findings = findings };
        }

        // ── Structural validation (definition JSON) ───────────────────────────────

        private static void ValidateStructure(WorkflowDefinitionSnapshot snapshot, List<PolicyValidationFinding> findings) {
            var stateNames   = snapshot.States.Select(s => s.Name).ToHashSet(System.StringComparer.OrdinalIgnoreCase);
            var transitions  = snapshot.Transitions;

            // ── S1: No initial state (Error) ─────────────────────────────────────
            var initialStates = snapshot.States.Where(s => s.IsInitial).ToList();
            if (initialStates.Count == 0)
                findings.Add(Error(null, null, null, HaleyFlowErrorCodes.NoInitialState, "No state is marked as initial. The relay cannot determine the start point."));

            // ── S2: Multiple initial states (Error) ──────────────────────────────
            if (initialStates.Count > 1)
                findings.Add(Error(null, null, null, HaleyFlowErrorCodes.MultipleInitialStates, $"Multiple states are marked as initial: {string.Join(", ", initialStates.Select(s => s.Name))}. Only one initial state is allowed."));

            // ── S3: Disconnected transitions — FromState or ToState not in states list (Error) ──
            foreach (var tr in transitions) {
                if (!stateNames.Contains(tr.FromState))
                    findings.Add(Error(tr.FromState, tr.EventCode, null, HaleyFlowErrorCodes.UnknownFromState, $"Transition event {tr.EventCode} references FromState '{tr.FromState}' which does not exist in the states list. Possible spelling mistake."));
                if (!stateNames.Contains(tr.ToState))
                    findings.Add(Error(tr.FromState, tr.EventCode, null, HaleyFlowErrorCodes.UnknownToState, $"Transition event {tr.EventCode} references ToState '{tr.ToState}' which does not exist in the states list. Possible spelling mistake."));
            }

            // ── S4: Duplicate event codes on same FromState (Error) ──────────────
            var duplicateEvents = transitions
                .GroupBy(t => (t.FromState.ToLowerInvariant(), t.EventCode))
                .Where(g => g.Count() > 1);
            foreach (var group in duplicateEvents)
                findings.Add(Error(group.Key.Item1, group.Key.EventCode, null, HaleyFlowErrorCodes.DuplicateEventCode, $"Event code {group.Key.EventCode} appears {group.Count()} times from state '{group.Key.Item1}'. Routing is ambiguous."));

            // ── S5: Terminal state with outgoing transitions (Error) ──────────────
            var outgoingStates = transitions.Select(t => t.FromState).ToHashSet(System.StringComparer.OrdinalIgnoreCase);
            foreach (var state in snapshot.States.Where(s => s.IsTerminal && outgoingStates.Contains(s.Name)))
                findings.Add(Error(state.Name, null, null, HaleyFlowErrorCodes.TerminalHasTransitions, $"State '{state.Name}' is marked as terminal but has outgoing transitions. A terminal state cannot be left."));

            // ── S6: Non-terminal state with no outgoing transitions (Error) ───────
            foreach (var state in snapshot.States.Where(s => !s.IsTerminal && !outgoingStates.Contains(s.Name)))
                findings.Add(Error(state.Name, null, null, HaleyFlowErrorCodes.StuckState, $"State '{state.Name}' is not terminal but has no outgoing transitions. The workflow will get permanently stuck here."));

            // ── S7: Unreachable states — no transition leads to this state (Warning) ──
            var reachableStates = transitions.Select(t => t.ToState).ToHashSet(System.StringComparer.OrdinalIgnoreCase);
            foreach (var state in snapshot.States.Where(s => !s.IsInitial && !reachableStates.Contains(s.Name)))
                findings.Add(Warn(state.Name, null, null, HaleyFlowErrorCodes.UnreachableState, $"State '{state.Name}' has no incoming transitions and is not the initial state. It can never be reached."));

            // ── S8: Circular-only state — self-loop is the ONLY outgoing transition (Error) ──
            var selfLoopOnlyStates = transitions
                .GroupBy(t => t.FromState, System.StringComparer.OrdinalIgnoreCase)
                .Where(g => g.All(t => string.Equals(t.ToState, t.FromState, System.StringComparison.OrdinalIgnoreCase)));
            foreach (var group in selfLoopOnlyStates)
                findings.Add(Error(group.Key, null, null, HaleyFlowErrorCodes.CircularOnly, $"State '{group.Key}' has only self-loop transitions and no exit. The workflow will loop forever with no way out."));

            // ── S9: Self-loop with other exits (Warning — likely intentional) ─────
            var selfLoopWithExits = transitions
                .GroupBy(t => t.FromState, System.StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Any(t => string.Equals(t.ToState, t.FromState, System.StringComparison.OrdinalIgnoreCase))
                         && g.Any(t => !string.Equals(t.ToState, t.FromState, System.StringComparison.OrdinalIgnoreCase)));
            foreach (var group in selfLoopWithExits)
                findings.Add(Warn(group.Key, null, null, HaleyFlowErrorCodes.SelfLoopWithExit, $"State '{group.Key}' has a self-loop transition but also has exit transitions. This is likely intentional (e.g. reminder/timeout) but verify it is not an accident."));
        }

        // ── Policy validation (policy JSON hooks) ────────────────────────────────

        private static void ValidatePolicy(WorkflowDefinitionSnapshot snapshot, List<PolicyValidationFinding> findings) {
            var stateNames = snapshot.States.Select(s => s.Name).ToHashSet(System.StringComparer.OrdinalIgnoreCase);

            // ── P1: Policy rule references a state not in the definition (Warning) ──
            // We infer "states referenced by policy" from transitions that have hooks (hooks come from policy rules).
            var policyStates = snapshot.Transitions
                .Where(t => t.Hooks.Count > 0 || t.ParamCodes.Count > 0)
                .Select(t => t.ToState)
                .Distinct(System.StringComparer.OrdinalIgnoreCase);
            foreach (var stateName in policyStates.Where(s => !stateNames.Contains(s)))
                findings.Add(Warn(stateName, null, null, HaleyFlowErrorCodes.PolicyUnknownState, $"Policy references state '{stateName}' which does not exist in the definition. This rule will never fire."));

            // ── P2: Hook-level checks per transition ─────────────────────────────
            foreach (var transition in snapshot.Transitions)
                ValidateTransitionHooks(transition, findings);
        }

        private static void ValidateTransitionHooks(SnapshotTransition transition, List<PolicyValidationFinding> findings) {
            var hooks = transition.Hooks.OrderBy(h => h.OrderSeq).ToList();
            if (hooks.Count == 0) return;

            var state = transition.ToState;
            int? via  = transition.EventCode;

            // ── H1: Non-blocking hooks with complete codes (Warning — ignored at runtime) ────
            foreach (var hook in hooks.Where(h => !h.Blocking && (h.CompleteSuccessCode.HasValue || h.CompleteFailureCode.HasValue)))
                findings.Add(Warn(state, via, hook.Route, HaleyFlowErrorCodes.NonBlockingWithComplete, $"Non-blocking hook '{hook.Route}' defines complete codes but they are ignored at runtime."));

            var blockingHooks = hooks.Where(h => h.Blocking).ToList();

            // ── H2: Multiple blocking hooks at the same order with complete codes (Error) ────
            var orderGroups = blockingHooks
                .Where(h => h.CompleteSuccessCode.HasValue || h.CompleteFailureCode.HasValue)
                .GroupBy(h => h.OrderSeq)
                .Where(g => g.Count() > 1);
            foreach (var group in orderGroups)
                findings.Add(Error(state, via, null, HaleyFlowErrorCodes.AmbiguousOrder, $"Multiple blocking hooks at order {group.Key} define complete codes: {string.Join(", ", group.Select(h => h.Route))}. Execution order is undefined."));

            // ── H3: Unreachable hooks — after any terminator (Warning) ──
            // Walk ALL hooks in order (blocking and non-blocking). Once a blocking hook with a success
            // code is encountered, everything after it is unreachable on the success path — including
            // non-blocking hooks, which will never be reached if the terminator fires first.
            // A failure-only blocking hook does NOT terminate on success, so hooks after it still run.
            int? terminatorOrder = null;
            foreach (var hook in hooks) {
                if (terminatorOrder.HasValue && hook.OrderSeq > terminatorOrder.Value) {
                    findings.Add(Warn(state, via, hook.Route, HaleyFlowErrorCodes.UnreachableHook, $"Hook '{hook.Route}' (order {hook.OrderSeq}) is unreachable — a previous blocking hook at order {terminatorOrder} terminates on success."));
                    continue;
                }
                // Only a blocking hook with a success code can be a terminator.
                if (hook.Blocking && hook.CompleteSuccessCode.HasValue)
                    terminatorOrder = hook.OrderSeq;
            }

            // ── H4: No failure path for blocking hook (Warning — code-side Tier 2/3 may handle it) ──
            foreach (var hook in blockingHooks) {
                if (!hook.CompleteFailureCode.HasValue && !transition.CompleteFailureCode.HasValue)
                    findings.Add(Warn(state, via, hook.Route, HaleyFlowErrorCodes.NoFailurePath, $"Blocking hook '{hook.Route}' has no failure complete code and the transition has no failure complete code. On failure the system cannot auto-advance — code-side handler (Tier 2/3) must decide the next step."));
            }
        }

        private static PolicyValidationFinding Warn(string? state, int? via, string? route, string code, string message) => new() {
            Severity = PolicyFindingSeverity.Warning, State = state ?? string.Empty, Via = via, Route = route, Code = code, Message = message
        };

        private static PolicyValidationFinding Error(string? state, int? via, string? route, string code, string message) => new() {
            Severity = PolicyFindingSeverity.Error, State = state ?? string.Empty, Via = via, Route = route, Code = code, Message = message
        };
    }
}
