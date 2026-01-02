using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IBlueprintReadDAL  {
        public interface IBlueprintReadDAL {
            Task<DbRow?> GetLatestDefVersionAsync(int envCode, string defName, DbExecutionLoad load = default);
            Task<DbRow?> GetDefVersionByIdAsync(long defVersionId, DbExecutionLoad load = default);

            Task<DbRows> ListStatesAsync(long defVersionId, DbExecutionLoad load = default);
            Task<DbRows> ListEventsAsync(long defVersionId, DbExecutionLoad load = default);
            Task<DbRows> ListTransitionsAsync(long defVersionId, DbExecutionLoad load = default);

            Task<DbRow?> GetPolicyByIdAsync(long policyId, DbExecutionLoad load = default);
            Task<DbRow?> GetPolicyByHashAsync(string hash, DbExecutionLoad load = default);

            Task<DbRow?> GetPolicyForStateAsync(long defId, long stateId, DbExecutionLoad load = default);
        }

    }
}
