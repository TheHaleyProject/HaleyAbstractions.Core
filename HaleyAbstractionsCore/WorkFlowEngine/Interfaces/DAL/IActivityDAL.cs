using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IActivityDAL {
        Task<DbRows> ListAllAsync(CancellationToken ct = default);
        Task<DbRow?> GetByNameAsync(string name, CancellationToken ct = default);
    }
}
