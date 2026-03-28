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

            if (!string.IsNullOrWhiteSpace(policyJson))
                MergePolicyHooks(policyJson!, transitions);

            return new WorkflowDefinitionSnapshot {
                DefinitionName = defName,
                EnvCode        = envCode,
                States         = states,
                Transitions    = transitions,
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

        private static void MergePolicyHooks(string policyJson, List<SnapshotTransition> transitions) {
            using var doc = JsonDocument.Parse(policyJson);
            var root = doc.RootElement;

            if (!root.TryGetProperty("rules", out var rules) || rules.ValueKind != JsonValueKind.Array) return;

            foreach (var rule in rules.EnumerateArray()) {
                var stateName = rule.GetString("state");
                if (string.IsNullOrWhiteSpace(stateName)) continue;

                int? ruleSuccessCode = null;
                int? ruleFailureCode = null;
                if (rule.TryGetProperty("complete", out var ruleComplete) && ruleComplete.ValueKind == JsonValueKind.Object) {
                    ruleSuccessCode = ruleComplete.GetInt("success");
                    ruleFailureCode = ruleComplete.GetInt("failure");
                }

                var hooks = rule.TryGetProperty("emit", out var emitArr) && emitArr.ValueKind == JsonValueKind.Array
                    ? ParseEmitHooks(emitArr)
                    : (IReadOnlyList<SnapshotHookRoute>)Array.Empty<SnapshotHookRoute>();

                for (var i = 0; i < transitions.Count; i++) {
                    var tr = transitions[i];
                    if (!string.Equals(tr.ToState, stateName, StringComparison.OrdinalIgnoreCase)) continue;

                    transitions[i] = new SnapshotTransition {
                        FromState           = tr.FromState,
                        ToState             = tr.ToState,
                        EventCode           = tr.EventCode,
                        EventName           = tr.EventName,
                        CompleteSuccessCode = ruleSuccessCode,
                        CompleteFailureCode = ruleFailureCode,
                        Hooks               = hooks,
                    };
                }
            }
        }

        private static IReadOnlyList<SnapshotHookRoute> ParseEmitHooks(JsonElement emitArr) {
            var result = new List<SnapshotHookRoute>();

            foreach (var h in emitArr.EnumerateArray()) {
                var route = h.GetString("route", "name");
                if (string.IsNullOrWhiteSpace(route)) continue;

                var label    = h.GetString("label") ?? string.Empty;
                var blocking = h.GetBool("blocking") ?? true;
                var order    = h.GetInt("order_seq", "orderSeq", "order") ?? 0;

                int? successCode = null;
                int? failureCode = null;
                if (h.TryGetProperty("complete", out var complete) && complete.ValueKind == JsonValueKind.Object) {
                    successCode = complete.GetInt("success");
                    failureCode = complete.GetInt("failure");
                }

                result.Add(new SnapshotHookRoute {
                    Route               = route!,
                    Label               = label,
                    Blocking            = blocking,
                    OrderSeq            = order,
                    CompleteSuccessCode = successCode,
                    CompleteFailureCode = failureCode,
                });
            }

            return result;
        }
    }
}
