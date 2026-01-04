using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintImporter {
        Task<long> ImportDefinitionJsonAsync(int envCode, string envDisplayName, string definitionJson, DbExecutionLoad load = default, CancellationToken ct = default);
        Task<long> ImportPolicyJsonAsync(int envCode, string envDisplayName, string policyJson, DbExecutionLoad load = default, CancellationToken ct = default);
    }
}