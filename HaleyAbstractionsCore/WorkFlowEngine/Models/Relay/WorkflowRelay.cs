using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    /// <summary>
    /// Local sequential workflow runner — zero infrastructure required.
    ///
    /// Loads definition + policy JSON via DefinitionJsonReader and owns the sequence.
    /// Business logic returns success/failure — the runner determines the next event code
    /// and advances automatically. Business logic has zero sequence knowledge.
    ///
    /// Three-tier routing (priority: Tier 3 → Tier 2 → Tier 1):
    ///   Tier 1 — Policy JSON complete codes (default, zero config in code)
    ///   Tier 2 — Code override on On/OnHook registration (when policy omits complete)
    ///   Tier 3 — Callback on On/OnHook registration (complex routing logic)
    ///
    /// Tradeoffs vs WorkflowEngine:
    ///   No timeout enforcement, no retry, no crash recovery, no ACK tracking.
    ///   Application is responsible for idempotency.
    ///
    /// Migration path:
    ///   Dev/early:   WorkflowRelay (local, zero infra, easy to debug)
    ///   Production:  ExecutorFlowBus → WorkflowEngine (same JSON, same event codes)
    /// </summary>
    public sealed class WorkflowRelay {
        private readonly WorkflowDefinitionSnapshot _snapshot;
        private readonly Dictionary<int,    RelayHandlerEntry>     _handlers     = new();
        private readonly Dictionary<string, RelayHookHandlerEntry> _hookHandlers = new(StringComparer.OrdinalIgnoreCase);
        private Func<int, string, Task<bool>>? _monitor;

        private WorkflowRelay(WorkflowDefinitionSnapshot snapshot) {
            _snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
        }

        /// <summary>
        /// Load definition + policy from JSON strings.
        /// policyJson is required for hook sequencing and auto-advance complete codes.
        /// Without policyJson only transition handlers run — hooks are unknown.
        /// </summary>
        public static WorkflowRelay FromJson(string definitionJson, string? policyJson = null, int envCode = 0) {
            var snapshot   = DefinitionJsonReader.ReadSnapshot(definitionJson, policyJson, envCode);
            var validation = PolicyValidator.Validate(snapshot);

            if (validation.HasCriticalErrors) {
                foreach (var f in validation.Findings.Where(f => f.Severity == PolicyFindingSeverity.Error))
                    Console.WriteLine($"[RELAY ERROR] {f}");
                var errors = string.Join("; ", validation.Findings.Where(f => f.Severity == PolicyFindingSeverity.Error).Select(f => f.ToString()));
                throw new InvalidOperationException($"Policy has critical errors and cannot be registered:\n{errors}");
            }

            if (validation.HasWarnings) {
                foreach (var f in validation.Findings.Where(f => f.Severity == PolicyFindingSeverity.Warning))
                    Console.WriteLine($"[RELAY WARNING] {f}");
            }

            return new WorkflowRelay(snapshot) { ValidationResult = validation };
        }

        /// <summary>The parsed snapshot — inspect states and transitions if needed.</summary>
        public WorkflowDefinitionSnapshot Snapshot => _snapshot;

        /// <summary>Validation result from registration time — check HasWarnings to surface policy issues.</summary>
        public PolicyValidationResult ValidationResult { get; private init; } = PolicyValidationResult.Ok();

        // ── Transition handler registration ───────────────────────────────────────

        /// <summary>Tier 1 — routing from policy JSON complete codes.</summary>
        public WorkflowRelay On(int eventCode, Func<RelayContext, Task<bool>> handler) {
            _handlers[eventCode] = new RelayHandlerEntry(handler);
            return this;
        }

        /// <summary>Tier 2 — code override when policy omits complete.</summary>
        public WorkflowRelay On(int eventCode, Func<RelayContext, Task<bool>> handler, int? successCode, int? failureCode) {
            _handlers[eventCode] = new RelayHandlerEntry(handler, successCode, failureCode);
            return this;
        }

        /// <summary>Tier 3 — callback for complex routing. Receives success/failure and returns the next event code (or null to stop).</summary>
        public WorkflowRelay On(int eventCode, Func<RelayContext, Task<bool>> handler, Func<bool, RelayContext, Task<int?>> onComplete) {
            _handlers[eventCode] = new RelayHandlerEntry(handler, onComplete: onComplete);
            return this;
        }

        // ── Hook handler registration ─────────────────────────────────────────────

        /// <summary>Tier 1 — routing from policy JSON complete codes.</summary>
        public WorkflowRelay OnHook(string route, Func<RelayContext, Task<bool>> handler) {
            if (string.IsNullOrWhiteSpace(route)) throw new ArgumentException("route is required.", nameof(route));
            _hookHandlers[route] = new RelayHookHandlerEntry(handler);
            return this;
        }

        /// <summary>Tier 2 — code override when policy omits complete.</summary>
        public WorkflowRelay OnHook(string route, Func<RelayContext, Task<bool>> handler, int? successCode, int? failureCode) {
            if (string.IsNullOrWhiteSpace(route)) throw new ArgumentException("route is required.", nameof(route));
            _hookHandlers[route] = new RelayHookHandlerEntry(handler, successCode, failureCode);
            return this;
        }

        /// <summary>Tier 3 — callback for complex routing.</summary>
        public WorkflowRelay OnHook(string route, Func<RelayContext, Task<bool>> handler, Func<bool, RelayContext, Task<int?>> onComplete) {
            if (string.IsNullOrWhiteSpace(route)) throw new ArgumentException("route is required.", nameof(route));
            _hookHandlers[route] = new RelayHookHandlerEntry(handler, onComplete: onComplete);
            return this;
        }

        // ── Monitor ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Optional intercept called before every handler (transition + hook).
        /// Return true to proceed, false to block — handler is not called and the run stops.
        /// Single place for checkpoints, logging, and debugging.
        /// </summary>
        public WorkflowRelay SetMonitor(Func<int, string, Task<bool>> monitor) {
            _monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
            return this;
        }

        // ── Execution ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Advance the workflow by one transition via eventCode.
        /// Calls the registered handler, then hook handlers in policy order.
        /// Auto-advances when a completing hook or transition handler resolves a next event code.
        /// Updates ctx.CurrentState on success.
        /// </summary>
        public async Task<RelayResult> NextAsync(RelayContext ctx, int eventCode, CancellationToken ct = default) {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));

            // For a new initiation, CurrentState is empty — resolve the initial state from the snapshot.
            if (string.IsNullOrWhiteSpace(ctx.CurrentState)) {
                var initial = _snapshot.States.FirstOrDefault(s => s.IsInitial);
                if (initial == null) return RelayResult.Blocked($"{HaleyFlowErrorCodes.RelayNoInitialState}: definition has no state marked is_initial.");
                ctx.CurrentState = initial.Name;
            }

            var transition = _snapshot.Transitions.FirstOrDefault(t =>
                string.Equals(t.FromState, ctx.CurrentState, StringComparison.OrdinalIgnoreCase)
                && t.EventCode == eventCode);

            if (transition == null)
                return RelayResult.Blocked($"{HaleyFlowErrorCodes.RelayInvalidTransition}: no transition from '{ctx.CurrentState}' via event code {eventCode}");

            // Monitor intercept before transition handler.
            if (_monitor != null && !await _monitor(eventCode, ctx.EntityRef))
                return RelayResult.Blocked(HaleyFlowErrorCodes.RelayBlockedByMonitor);

            // Resolve parameters for this transition so handlers can read approval rules, roles, etc.
            ctx.Parameters = transition.ParamCodes.Count > 0
                ? _snapshot.Parameters.Where(p => transition.ParamCodes.Contains(p.Code, System.StringComparer.OrdinalIgnoreCase)).ToList()
                : System.Array.Empty<SnapshotParameter>();

            // Run the transition handler if registered.
            // State has NOT advanced yet — handler executes in the context of the current (from) state.
            bool transitionSucceeded = true;
            int? transitionNextCode = null;
            if (_handlers.TryGetValue(eventCode, out var entry)) {
                ct.ThrowIfCancellationRequested();
                transitionSucceeded = await entry.Handler(ctx);

                // Failure — skip all hooks and fire failure event immediately.
                if (!transitionSucceeded) {
                    var failCode = await ResolveNextCode(false, ctx, entry, transition.CompleteSuccessCode, transition.CompleteFailureCode);
                    if (failCode.HasValue) {
                        ctx.CurrentState = transition.ToState;
                        return await NextAsync(ctx, failCode.Value, ct);
                    }
                    return RelayResult.Blocked($"{HaleyFlowErrorCodes.RelayTransitionFailed}: event {eventCode}");
                }

                // Success — hold the next code, run hooks first.
                transitionNextCode = await ResolveNextCode(true, ctx, entry, transition.CompleteSuccessCode, transition.CompleteFailureCode);
            }

            // Advance to ToState — hooks fire because we arrived at this state.
            ctx.CurrentState = transition.ToState;

            var groupedOrders = transition.Hooks
                .OrderBy(h => h.OrderSeq)
                .GroupBy(h => h.OrderSeq)
                .Select(g => new {
                    Order = g.Key,
                    Hooks = g.ToList(),
                })
                .ToList();

            int? pendingGateSuccessCode = null;

            foreach (var group in groupedOrders) {
                ct.ThrowIfCancellationRequested();

                var gateHooks = group.Hooks.Where(h => h.Type == HookType.Gate).ToList();
                var effectHooks = group.Hooks.Where(h => h.Type == HookType.Effect).ToList();

                // A previously resolved gate-success short-circuits later gate orders.
                // Only effect hooks explicitly marked send=always remain reachable on the success path.
                if (pendingGateSuccessCode.HasValue) {
                    await ExecuteEffectHooksAsync(ctx, effectHooks.Where(h => h.SendAlways), ct);
                    continue;
                }

                var gatePhase = await ExecuteGatePhaseAsync(ctx, transition, eventCode, gateHooks, ct);

                if (gatePhase.Failed) {
                    if (gatePhase.IsBlocked)
                        return RelayResult.Blocked(gatePhase.BlockReason ?? HaleyFlowErrorCodes.RelayBlockedByMonitor);

                    if (gatePhase.NextCode.HasValue)
                        return await NextAsync(ctx, gatePhase.NextCode.Value, ct);

                    ctx.CurrentState = transition.FromState;
                    return RelayResult.Blocked(gatePhase.BlockReason ?? $"{HaleyFlowErrorCodes.RelayBlockingHookFailed}: order {group.Order}");
                }

                // Same-order effects run only when the gate phase completed successfully.
                await ExecuteEffectHooksAsync(ctx, effectHooks, ct);

                if (gatePhase.NextCode.HasValue) {
                    pendingGateSuccessCode = gatePhase.NextCode.Value;
                    continue;
                }
            }

            // If a gate resolved a success code, fire it now (all reachable effect phases have run).
            if (pendingGateSuccessCode.HasValue)
                return await NextAsync(ctx, pendingGateSuccessCode.Value, ct);

            // All hooks complete with no gate termination — fall back to transition handler's next code.
            if (transitionNextCode.HasValue)
                return await NextAsync(ctx, transitionNextCode.Value, ct);

            return RelayResult.Ok(ctx.CurrentState);
        }

        // ── Routing resolution ────────────────────────────────────────────────────

        private static async Task<int?> ResolveNextCode(bool succeeded, RelayContext ctx, RelayHandlerEntry entry, int? policySuccess, int? policyFailure) {
            // Tier 3 — callback wins.
            if (entry.OnComplete != null) return await entry.OnComplete(succeeded, ctx);
            // Tier 2 — code override.
            if (entry.SuccessCode.HasValue || entry.FailureCode.HasValue)
                return succeeded ? entry.SuccessCode : entry.FailureCode;
            // Tier 1 — policy JSON.
            return succeeded ? policySuccess : policyFailure;
        }

        private static async Task<int?> ResolveNextCode(bool succeeded, RelayContext ctx, RelayHookHandlerEntry entry, int? policySuccess, int? policyFailure) {
            if (entry.OnComplete != null) return await entry.OnComplete(succeeded, ctx);
            if (entry.SuccessCode.HasValue || entry.FailureCode.HasValue)
                return succeeded ? entry.SuccessCode : entry.FailureCode;
            return succeeded ? policySuccess : policyFailure;
        }

        private async Task ExecuteEffectHooksAsync(RelayContext ctx, IEnumerable<SnapshotHookRoute> hooks, CancellationToken ct) {
            foreach (var hook in hooks) {
                ct.ThrowIfCancellationRequested();
                if (!_hookHandlers.TryGetValue(hook.Route, out var effectEntry)) continue;

                ctx.Parameters = hook.ParamCodes.Count > 0
                    ? _snapshot.Parameters.Where(p => hook.ParamCodes.Contains(p.Code, System.StringComparer.OrdinalIgnoreCase)).ToList()
                    : System.Array.Empty<SnapshotParameter>();

                await effectEntry.Handler(ctx);
            }
        }

        private async Task<GatePhaseResult> ExecuteGatePhaseAsync(RelayContext ctx, SnapshotTransition transition, int eventCode, IReadOnlyList<SnapshotHookRoute> gateHooks, CancellationToken ct) {
            if (gateHooks.Count == 0) return GatePhaseResult.None;

            int? successCode = null;
            int? failureCode = null;
            var failed = false;

            foreach (var hook in gateHooks) {
                ct.ThrowIfCancellationRequested();

                if (_monitor != null && !await _monitor(eventCode, ctx.EntityRef))
                    return GatePhaseResult.Blocked(HaleyFlowErrorCodes.RelayBlockedByMonitor);

                if (!_hookHandlers.TryGetValue(hook.Route, out var gateEntry)) continue;

                ctx.Parameters = hook.ParamCodes.Count > 0
                    ? _snapshot.Parameters.Where(p => hook.ParamCodes.Contains(p.Code, System.StringComparer.OrdinalIgnoreCase)).ToList()
                    : System.Array.Empty<SnapshotParameter>();

                var succeeded = await gateEntry.Handler(ctx);
                var resolvedCode = await ResolveNextCode(succeeded, ctx, gateEntry, hook.CompleteSuccessCode, hook.CompleteFailureCode);

                if (!succeeded) {
                    failed = true;
                    failureCode ??= resolvedCode ?? transition.CompleteFailureCode;
                    continue;
                }

                successCode ??= resolvedCode;
            }

            if (failed)
                return failureCode.HasValue
                    ? GatePhaseResult.Fail(failureCode.Value)
                    : GatePhaseResult.Blocked(HaleyFlowErrorCodes.RelayBlockingHookFailed);

            return successCode.HasValue ? GatePhaseResult.Success(successCode.Value) : GatePhaseResult.None;
        }


        // ── Internal entry types ──────────────────────────────────────────────────

        private readonly record struct GatePhaseResult(bool Failed, bool IsBlocked, int? NextCode, string? BlockReason) {
            public static GatePhaseResult None => new(false, false, null, null);
            public static GatePhaseResult Blocked(string reason) => new(true, true, null, reason);
            public static GatePhaseResult Success(int nextCode) => new(false, false, nextCode, null);
            public static GatePhaseResult Fail(int nextCode) => new(true, false, nextCode, null);
        }

        private sealed class RelayHandlerEntry {
            public Func<RelayContext, Task<bool>>              Handler    { get; }
            public int?                                        SuccessCode { get; }
            public int?                                        FailureCode { get; }
            public Func<bool, RelayContext, Task<int?>>?       OnComplete  { get; }

            public RelayHandlerEntry(Func<RelayContext, Task<bool>> handler, int? successCode = null, int? failureCode = null, Func<bool, RelayContext, Task<int?>>? onComplete = null) {
                Handler     = handler;
                SuccessCode = successCode;
                FailureCode = failureCode;
                OnComplete  = onComplete;
            }
        }

        private sealed class RelayHookHandlerEntry {
            public Func<RelayContext, Task<bool>>              Handler    { get; }
            public int?                                        SuccessCode { get; }
            public int?                                        FailureCode { get; }
            public Func<bool, RelayContext, Task<int?>>?       OnComplete  { get; }

            public RelayHookHandlerEntry(Func<RelayContext, Task<bool>> handler, int? successCode = null, int? failureCode = null, Func<bool, RelayContext, Task<int?>>? onComplete = null) {
                Handler     = handler;
                SuccessCode = successCode;
                FailureCode = failureCode;
                OnComplete  = onComplete;
            }
        }
    }
}
