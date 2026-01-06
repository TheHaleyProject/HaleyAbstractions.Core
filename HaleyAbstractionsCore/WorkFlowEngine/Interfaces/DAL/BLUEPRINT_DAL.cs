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
        Task<DbRow?> GetDefVersionByParentAndHashAsync(int definitionId, string hash, DbExecutionLoad load = default);

        Task<DbRow?> GetDefVersionByIdAsync(long defVersionId, DbExecutionLoad load = default);

        Task<DbRows> ListStatesAsync(long defVersionId, DbExecutionLoad load = default);
        Task<DbRows> ListEventsAsync(long defVersionId, DbExecutionLoad load = default);
        Task<DbRows> ListTransitionsAsync(long defVersionId, DbExecutionLoad load = default);

        Task<DbRow?> GetPolicyByIdAsync(long policyId, DbExecutionLoad load = default);
        Task<DbRow?> GetPolicyByHashAsync(string hash, DbExecutionLoad load = default);

        public Task<DbRow?> GetPolicyForDefinition(long definitionId, DbExecutionLoad load = default);
        public Task<DbRow?> GetPolicyForDefVersion(long defVersionId, DbExecutionLoad load = default);
    }
    public interface IBlueprintWriteDAL {
        Task<int> EnsureEnvironmentByCodeAsync(int envCode, string envDisplayName, DbExecutionLoad load = default);
        Task<int> EnsureDefinitionByEnvIdAsync(int envId, string defDisplayName, string? description, DbExecutionLoad load = default);
        Task<long> InsertDefVersionAsync(int definitionId, int version, string data, string hash, DbExecutionLoad load = default);
        Task<int> EnsureCategoryByNameAsync(string displayName, DbExecutionLoad load = default);
        Task<int> InsertEventAsync(long defVersionId, string displayName, int code, DbExecutionLoad load = default);
        Task<int> InsertStateAsync(long defVersionId, int categoryId, string displayName, uint flags, int? timeoutMinutes, uint timeoutMode, long? timeoutEventId, DbExecutionLoad load = default);
        Task<int> InsertTransitionAsync(long defVersionId, int fromStateId, int toStateId, int eventId, DbExecutionLoad load = default);
        Task<long> EnsurePolicyByHashAsync(string hash, string content, DbExecutionLoad load = default);
        Task<int> AttachPolicyToDefinitionByEnvCodeAndDefNameAsync(int envCode, string defName, long policyId, DbExecutionLoad load = default);
    }

}
