using Haley.Enums;
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
    /// WorkflowRelay.FromJson and any other local consumer of the definition JSON call this -
    /// so if the JSON format evolves, only this class needs updating.
    ///
    /// The engine (BluePrintImporter) does its own parse for DB write purposes. This class
    /// owns the read-only projection path (snapshot + relay) - no DB involved.
    /// </summary>
    public static class DefinitionJsonReader {
        /// <summary>
        /// Parse a definition JSON string into a WorkflowDefinitionSnapshot.
        /// Pass policyJson to merge hook routes from policy rules into transitions.
        /// envCode is not in the JSON - pass it from the call site (defaults to 0 if unknown).
        /// </summary>
        public static WorkflowDefinitionSnapshot ReadSnapshot(string definitionJson, string? policyJson = null, int envCode = 0) {
            if (string.IsNullOrWhiteSpace(definitionJson)) throw new ArgumentNullException(nameof(definitionJson));

            using var defDoc = JsonDocument.Parse(definitionJson);
            var root = defDoc.RootElement;

            if (root.ValueKind != JsonValueKind.Object) throw new InvalidOperationException("Definition JSON root must be an object.");
            if (root.TryGetProperty("definition", out _)) throw new InvalidOperationException("Legacy definition wrapper is not supported. Move definition fields to the top level.");

            var defName = root.GetString("name") ?? throw new InvalidOperationException("Definition JSON must contain top-level 'name'.");
            var transitions = ParseTransitions(root);
            var states = ParseStates(root, transitions);

            var parameters = new List<SnapshotParameter>();
            if (!string.IsNullOrWhiteSpace(policyJson))
                MergePolicyHooks(policyJson!, transitions, parameters);

            return new WorkflowDefinitionSnapshot {
                DefinitionName = defName,
                EnvCode = envCode,
                States = states,
                Transitions = transitions,
                Parameters = parameters,
            };
        }

        private static List<SnapshotTransition> ParseTransitions(JsonElement root) {
            var result = new List<SnapshotTransition>();
            if (!root.TryGetProperty("transitions", out var arr) || arr.ValueKind != JsonValueKind.Array) return result;

            foreach (var t in arr.EnumerateArray()) {
                if (t.ValueKind != JsonValueKind.Object) throw new InvalidOperationException("Each transition must be an object.");

                var from = t.GetString("from") ?? throw new InvalidOperationException("Each transition must contain 'from'.");
                var to = t.GetString("to") ?? throw new InvalidOperationException("Each transition must contain 'to'.");
                var code = t.GetInt("event") ?? throw new InvalidOperationException("Each transition must contain integer 'event'.");
                var name = t.GetString("eventName") ?? string.Empty;

                result.Add(new SnapshotTransition {
                    FromState = from,
                    ToState = to,
                    EventCode = code,
                    EventName = name,
                    Hooks = Array.Empty<SnapshotHookRoute>(),
                });
            }

            return result;
        }

        private static IReadOnlyList<SnapshotState> ParseStates(JsonElement root, List<SnapshotTransition> transitions) {
            var result = new List<SnapshotState>();
            if (!root.TryGetProperty("states", out var arr) || arr.ValueKind != JsonValueKind.Array) return result;

            var hasOutgoing = new HashSet<string>(transitions.Select(t => t.FromState), StringComparer.OrdinalIgnoreCase);

            foreach (var s in arr.EnumerateArray()) {
                if (s.ValueKind != JsonValueKind.Object) throw new InvalidOperationException("Each state must be an object.");

                var name = s.GetString("name") ?? throw new InvalidOperationException("Each state must contain 'name'.");
                var isInitial = s.GetBool("is_initial") ?? false;
                var isTerminal = s.GetBool("is_final") ?? !hasOutgoing.Contains(name);

                result.Add(new SnapshotState {
                    Name = name,
                    IsInitial = isInitial,
                    IsTerminal = isTerminal,
                });
            }

            return result;
        }

        private static void MergePolicyHooks(string policyJson, List<SnapshotTransition> transitions, List<SnapshotParameter> parameters) {
            using var doc = JsonDocument.Parse(policyJson);
            var root = doc.RootElement;

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

            var claimed = new HashSet<int>();

            foreach (var rule in rules.EnumerateArray()) {
                var via = rule.GetInt("via");
                if (!via.HasValue) continue;

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

            foreach (var rule in rules.EnumerateArray()) {
                var via = rule.GetInt("via");
                if (via.HasValue) continue;

                var stateName = rule.GetString("state");
                if (string.IsNullOrWhiteSpace(stateName)) continue;

                var (successCode, failureCode, paramCodes, hooks) = ParseRuleBody(rule);

                for (var i = 0; i < transitions.Count; i++) {
                    if (claimed.Contains(i)) continue;
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
                    if (pc.ValueKind != JsonValueKind.String) throw new InvalidOperationException("Rule params must be a string array of parameter codes.");
                    var c = pc.GetString();
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
                FromState = tr.FromState,
                ToState = tr.ToState,
                EventCode = tr.EventCode,
                EventName = tr.EventName,
                CompleteSuccessCode = successCode,
                CompleteFailureCode = failureCode,
                Hooks = hooks,
                ParamCodes = paramCodes,
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
                JsonValueKind.Object => ParseObjectAsDictionary(el),
                JsonValueKind.Array => el.EnumerateArray().Select(ParseJsonValue).ToList(),
                JsonValueKind.String => el.GetString(),
                JsonValueKind.Number => el.TryGetInt64(out var l) ? (object?)l : el.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => null,
            };
        }

        private static IReadOnlyList<SnapshotHookRoute> ParseEmitHooks(JsonElement emitArr, IReadOnlyList<string> ruleParamCodes) {
            var result = new List<SnapshotHookRoute>();

            foreach (var h in emitArr.EnumerateArray()) {
                if (h.ValueKind != JsonValueKind.Object) throw new InvalidOperationException("Each emit hook must be an object.");
                if (h.TryGetProperty("orderSeq", out _) || h.TryGetProperty("order_seq", out _)) throw new InvalidOperationException("Legacy emit order aliases are not supported. Use 'order'.");

                var route = h.GetString("route") ?? throw new InvalidOperationException("Each emit hook must contain 'route'.");
                var label = h.GetString("label") ?? string.Empty;
                var typeStr = h.GetString("type");
                var hookType = !string.IsNullOrWhiteSpace(typeStr)
                    ? (string.Equals(typeStr, "effect", StringComparison.OrdinalIgnoreCase) ? HookType.Effect : HookType.Gate)
                    : ((h.GetBool("blocking") ?? true) ? HookType.Gate : HookType.Effect);
                var order = h.GetInt("order") ?? 999;

                int? successCode = null;
                int? failureCode = null;
                if (h.TryGetProperty("complete", out var complete) && complete.ValueKind == JsonValueKind.Object) {
                    successCode = complete.GetInt("success");
                    failureCode = complete.GetInt("failure");
                }

                IReadOnlyList<string> hookParamCodes = ruleParamCodes;
                if (h.TryGetProperty("params", out var hookParams) && hookParams.ValueKind == JsonValueKind.Array) {
                    var codes = new List<string>();
                    foreach (var pc in hookParams.EnumerateArray()) {
                        if (pc.ValueKind != JsonValueKind.String) throw new InvalidOperationException("Hook params must be a string array of parameter codes.");
                        var c = pc.GetString();
                        if (!string.IsNullOrWhiteSpace(c)) codes.Add(c!);
                    }
                    hookParamCodes = codes;
                }

                result.Add(new SnapshotHookRoute {
                    Route = route,
                    Label = label,
                    Type = hookType,
                    OrderSeq = order,
                    CompleteSuccessCode = successCode,
                    CompleteFailureCode = failureCode,
                    ParamCodes = hookParamCodes,
                });
            }

            return result;
        }
    }
}
