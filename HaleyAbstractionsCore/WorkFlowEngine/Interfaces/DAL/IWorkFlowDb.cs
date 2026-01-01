using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowDb : IAsyncDisposable {
        Task<int> ExecAsync(string sql, CancellationToken ct = default, params DbArg[] args);
        Task<T?> ScalarAsync<T>(string sql, CancellationToken ct = default, params DbArg[] args);
        Task<DbRow?> RowAsync(string sql, CancellationToken ct = default, params DbArg[] args);
        Task<DbRows> RowsAsync(string sql, CancellationToken ct = default, params DbArg[] args);
    }
}
