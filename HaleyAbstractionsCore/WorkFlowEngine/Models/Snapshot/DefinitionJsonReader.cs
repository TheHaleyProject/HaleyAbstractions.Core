using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Haley.Models {
    /// <summary>
    /// Parses workflow definition + policy JSON into a <see cref="WorkflowDefinitionSnapshot"/>.
    ///
    /// Single source of truth for definition JSON parsing outside the engine.
    /// WorkflowRelay.FromJson and any other local consumer of the definition JSON call this —
    /// so if the JSON format evolves, only this class needs updating.
    ///
    /// The engine (BluePrintImporter) does its own parse for DB write purposes. This class
    /// owns the read-only projection path (snapshot + relay) — no DB involved.
    /// </summary>
    public static class DefinitionJsonReader {
        /// <summary>
        /// Parse a definition JSON string into a WorkflowDefinitionSnapshot.
        /// Pass policyJson to merge hook routes from policy rules into transitions.
        /// envCode is not in the JSON — pass it from the call site (defaults to 0 if unknown).
        /// </summary>
        public static WorkflowDefinitionSnapshot ReadSnapshot(string definitionJson, string? policyJson = null, int envCode = 0) {
            if (string.IsNullOrWhiteSpace(definitionJson)) throw new ArgumentNullException(nameof(definitionJson));

            using var defDoc = JsonDocument.Parse(definitionJson);
            var root = defDoc.RootElement;

            var defName     = root.GetString("name", "displayName", "defName", "definitionName") ?? string.Empty;
            var transitions = ParseTransitions(root);
            var states      = ParseStates(root, transitions);

            var parameters = new List<SnapshotParameter>();
            if (!string.IsNullOrWhiteSpace(policyJson))
                MergePolicyHooks(policyJson!, transitions, parameters);

            return new WorkflowDefinitionSnapshot {
                DefinitionName = defName,
                EnvCode        = envCode,
                States         = states,
                Transitions    = transitions,
                Parameters     = parameters,
            };
        }

        // ── Definition parsers ────────────────────────────────────────────────────

        private static List<SnapshotTransition> ParseTransitions(JsonElement root) {
            var result = new List<SnapshotTransition>();
            if (!root.TryGetProperty("transitions", out var arr) || arr.ValueKind != JsonValueKind.Array) return result;

            foreach (var t in arr.EnumerateArray()) {
                var from = t.GetString("from", "fromState");
                var to   = t.GetString("to",   "toState");
                var code = t.GetInt("event", "eventCode");
                var name = t.GetString("eventName", "event_name") ?? string.Empty;

                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || !code.HasValue) continue;

                result.Add(new SnapshotTransition {
                    FromState = from!,
                    ToState   = to!,
                    EventCode = code.Value,
                    EventName = name,
                    Hooks     = Array.Empty<SnapshotHookRoute>(),
                });
            }

            return result;
        }

        private static IReadOnlyList<SnapshotState> ParseStates(JsonElement root, List<SnapshotTransition> transitions) {
            var result = new List<SnapshotState>();
            if (!root.TryGetProperty("states", out var arr) || arr.ValueKind != JsonValueKind.Array) return result;

            var hasOutgoing = new HashSet<string>(transitions.Select(t => t.FromState), StringComparer.OrdinalIgnoreCase);

            foreach (var s in arr.EnumerateArray()) {
                var name = s.GetString("name", "displayName");
                if (string.IsNullOrWhiteSpace(name)) continue;

                var isInitial  = s.GetBool("isInitial",  "is_initial")  ?? false;
                var isTerminal = s.GetBool("isFinal",    "is_final",    "isTerminal", "is_terminal") ?? !hasOutgoing.Contains(name!);

                result.Add(new SnapshotState {
                    Name       = name!,
                    IsInitial  = isInitial,
                    IsTerminal = isTerminal,
                });
            }

            return result;
        }

        // ── Policy merger ─────────────────────────────────────────────────────────

        private static void MergePolicyHooks(string policyJson, List<SnapshotTransition> transitions, List<SnapshotParameter> parameters) {
            using var doc = JsonDocument.Parse(policyJson);
            var root = doc.RootElement;

            // Parse top-level params catalog.
            if (root.TryGetProperty("params", out var paramsArr) && paramsArr.ValueKind == JsonValueKind.Array) {
                foreach (var p in paramsArr.EnumerateArray()) {
                    var code = p.GetString("code");
                    if (string.IsNullOrWhiteSpace(code)) continue;
                    var data = p.TryGetProperty("data", out var dataEl) && dataEl.ValueKind == JsonValueKind.Object
                        ? ParseObjectAsDictionary(dataEl)
                        : (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>();
                    parameters.Add(new SnapshotParameter { Code = code!, Data = data });
                }
            }

            if (!root.TryGetProperty("rules", out var rules) || rules.ValueKind != JsonValueKind.Array) return;

            // Two-pass merge:
            //   Pass 1 — via-specific rules (via is set). Apply first and record which transition indices were claimed.
            //   Pass 2 — general rules (no via). Only apply to transitions not already claimed by a via-specific rule.
            //
            // This means: if a state has both a via-specific rule (e.g. via=2005) and a general rule,
            // the transition arriving via 2005 uses the via-specific rule and the general rule is skipped for it.
            // Other transitions arriving at the same state use the general rule.

            var claimed = new HashSet<int>(); // indices of transitions already assigned by a via-specific rule

            // ── Pass 1: via-specific rules ────────────────────────────────────────
            foreach (var rule in rules.EnumerateArray()) {
                var via = rule.GetInt("via");
                if (!via.HasValue) continue;  // skip general rules in this pass

                var stateName = rule.GetString("state");
                if (string.IsNullOrWhiteSpace(stateName)) continue;

                var (successCode, failureCode, paramCodes, hooks) = ParseRuleBody(rule);

                for (var i = 0; i < transitions.Count; i++) {
                    var tr = transitions[i];
                    if (!string.Equals(tr.ToState, stateName, StringComparison.OrdinalIgnoreCase)) continue;
                    if (tr.EventCode != via.Value) continue;

                    transitions[i] = ApplyRule(tr, successCode, failureCode, hooks, paramCodes);
                    claimed.Add(i);
                }
            }

            // ── Pass 2: general rules (no via) — skip claimed transitions ─────────
            foreach (var rule in rules.EnumerateArray()) {
                var via = rule.GetInt("via");
                if (via.HasValue) continue;  // skip via-specific rules in this pass

                var stateName = rule.GetString("state");
                if (string.IsNullOrWhiteSpace(stateName)) continue;

                var (successCode, failureCode, paramCodes, hooks) = ParseRuleBody(rule);

                for (var i = 0; i < transitions.Count; i++) {
                    if (claimed.Contains(i)) continue;  // already claimed by a via-specific rule
                    var tr = transitions[i];
                    if (!string.Equals(tr.ToState, stateName, StringComparison.OrdinalIgnoreCase)) continue;

                    transitions[i] = ApplyRule(tr, successCode, failureCode, hooks, paramCodes);
                }
            }
        }

        private static (int? successCode, int? failureCode, IReadOnlyList<string> paramCodes, IReadOnlyList<SnapshotHookRoute> hooks) ParseRuleBody(JsonElement rule) {
            int? successCode = null;
            int? failureCode = null;
            if (rule.TryGetProperty("complete", out var complete) && complete.ValueKind == JsonValueKind.Object) {
                successCode = complete.GetInt("success");
                failureCode = complete.GetInt("failure");
            }

            IReadOnlyList<string> paramCodes = Array.Empty<string>();
            if (rule.TryGetProperty("params", out var ruleParams) && ruleParams.ValueKind == JsonValueKind.Array) {
                var codes = new List<string>();
                foreach (var pc in ruleParams.EnumerateArray()) {
                    var c = pc.ValueKind == JsonValueKind.String ? pc.GetString() : pc.GetString("code");
                    if (!string.IsNullOrWhiteSpace(c)) codes.Add(c!);
                }
                paramCodes = codes;
            }

            var hooks = rule.TryGetProperty("emit", out var emitArr) && emitArr.ValueKind == JsonValueKind.Array
                ? ParseEmitHooks(emitArr, paramCodes)
                : (IReadOnlyList<SnapshotHookRoute>)Array.Empty<SnapshotHookRoute>();

            return (successCode, failureCode, paramCodes, hooks);
        }

        private static SnapshotTransition ApplyRule(SnapshotTransition tr, int? successCode, int? failureCode, IReadOnlyList<SnapshotHookRoute> hooks, IReadOnlyList<string> paramCodes) {
            return new SnapshotTransition {
                FromState           = tr.FromState,
                ToState             = tr.ToState,
                EventCode           = tr.EventCode,
                EventName           = tr.EventName,
                CompleteSuccessCode = successCode,
                CompleteFailureCode = failureCode,
                Hooks               = hooks,
                ParamCodes          = paramCodes,
            };
        }

        private static IReadOnlyDictionary<string, object?> ParseObjectAsDictionary(JsonElement obj) {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var prop in obj.EnumerateObject())
                dict[prop.Name] = ParseJsonValue(prop.Value);
            return dict;
        }

        private static object? ParseJsonValue(JsonElement el) {
            return el.ValueKind switch {
                JsonValueKind.Object  => ParseObjectAsDictionary(el),
                JsonValueKind.Array   => el.EnumerateArray().Select(ParseJsonValue).ToList(),
                JsonValueKind.String  => el.GetString(),
                JsonValueKind.Number  => el.TryGetInt64(out var l) ? (object?)l : el.GetDouble(),
                JsonValueKind.True    => true,
                JsonValueKind.False   => false,
                _                     => null,
            };
        }

        private static IReadOnlyList<SnapshotHookRoute> ParseEmitHooks(JsonElement emitArr, IReadOnlyList<string> ruleParamCodes) {
            var result = new List<SnapshotHookRoute>();

            foreach (var h in emitArr.EnumerateArray()) {
                var route = h.GetString("route", "name");
                if (string.IsNullOrWhiteSpace(route)) continue;

                var label    = h.GetString("label") ?? string.Empty;
                var blocking = h.GetBool("blocking") ?? true;
                // No order specified — runs last (after all explicitly ordered hooks).
                var order    = h.GetInt("order_seq", "orderSeq", "order") ?? int.MaxValue;

                int? successCode = null;
                int? failureCode = null;
                if (h.TryGetProperty("complete", out var complete) && complete.ValueKind == JsonValueKind.Object) {
                    successCode = complete.GetInt("success");
                    failureCode = complete.GetInt("failure");
                }

                // Hook-own params take priority; fall back to parent rule params if hook defines none.
                IReadOnlyList<string> hookParamCodes = ruleParamCodes;
                if (h.TryGetProperty("params", out var hookParams) && hookParams.ValueKind == JsonValueKind.Array) {
                    var codes = new List<string>();
                    foreach (var pc in hookParams.EnumerateArray()) {
                        var c = pc.ValueKind == JsonValueKind.String ? pc.GetString() : pc.GetString("code");
                        if (!string.IsNullOrWhiteSpace(c)) codes.Add(c!);
                    }
                    hookParamCodes = codes;
                }

                result.Add(new SnapshotHookRoute {
                    Route               = route!,
                    Label               = label,
                    Blocking            = blocking,
                    OrderSeq            = order,
                    CompleteSuccessCode = successCode,
                    CompleteFailureCode = failureCode,
                    ParamCodes          = hookParamCodes,
                });
            }

            return result;
        }
    }
}
