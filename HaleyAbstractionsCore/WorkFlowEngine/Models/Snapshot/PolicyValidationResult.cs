using System.Collections.Generic;
using System.Linq;

namespace Haley.Models {
    /// <summary>
    /// Result of PolicyValidator.Validate — contains all findings across all rules.
    /// HasCriticalErrors = true means the policy must be rejected at registration time.
    /// </summary>
    public sealed class PolicyValidationResult {
        public IReadOnlyList<PolicyValidationFinding> Findings     { get; init; } = System.Array.Empty<PolicyValidationFinding>();
        public bool                                   IsValid      => !HasCriticalErrors;
        public bool                                   HasCriticalErrors => Findings.Any(f => f.Severity == PolicyFindingSeverity.Error);
        public bool                                   HasWarnings  => Findings.Any(f => f.Severity == PolicyFindingSeverity.Warning);

        public static PolicyValidationResult Ok() => new() { Findings = System.Array.Empty<PolicyValidationFinding>() };
    }
}
