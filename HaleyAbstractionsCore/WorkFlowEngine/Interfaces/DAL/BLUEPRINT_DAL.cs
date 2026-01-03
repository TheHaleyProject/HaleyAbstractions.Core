using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintReadDAL {
        Task<DbRow?> GetLatestDefVersionByEnvCodeAndDefNameAsync(int envCode, string defName, DbExecutionLoad load = default);
        Task<DbRow?> GetLatestDefVersionByEnvNameAndDefNameAsync(string envName, string defName, DbExecutionLoad load = default);
        Task<DbRow?> GetLatestDefVersionByDefinitionGuidAsync(string defGuid, DbExecutionLoad load = default);
        Task<DbRow?> GetLatestDefVersionByLineFromDefVersionIdAsync(long defVersionId, DbExecutionLoad load = default);
        Task<DbRow?> GetLatestDefVersionByLineFromDefVersionGuidAsync(string defVersionGuid, DbExecutionLoad load = default);
        Task<int?> GetNextDefVersionNumberByEnvCodeAndDefNameAsync(int envCode, string defName, DbExecutionLoad load = default);

        Task<DbRow?> GetDefVersionByIdAsync(long defVersionId, DbExecutionLoad load = default);

        Task<DbRows> ListStatesAsync(long defVersionId, DbExecutionLoad load = default);
        Task<DbRows> ListEventsAsync(long defVersionId, DbExecutionLoad load = default);
        Task<DbRows> ListTransitionsAsync(long defVersionId, DbExecutionLoad load = default);

        Task<DbRow?> GetPolicyByIdAsync(long policyId, DbExecutionLoad load = default);
        Task<DbRow?> GetPolicyByHashAsync(string hash, DbExecutionLoad load = default);

        // policy attached to state/transition (your "policy json included on transition" concept)
        Task<DbRow?> GetPolicyForStateAsync(long definitionId, long stateId, DbExecutionLoad load = default);
    }
}
