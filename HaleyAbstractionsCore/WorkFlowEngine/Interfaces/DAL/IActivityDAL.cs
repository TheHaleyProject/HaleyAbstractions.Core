using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IActivityDAL {
        Task<DbRows> ListAllAsync(DbExecutionLoad load = default);
        Task<DbRow?> GetByNameAsync(string name, DbExecutionLoad load = default);
    }
}
