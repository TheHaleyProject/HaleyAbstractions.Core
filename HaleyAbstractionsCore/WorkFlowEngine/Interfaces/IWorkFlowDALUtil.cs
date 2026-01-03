using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowDALUtil : IAsyncDisposable {
        Task<int> ExecAsync(string sql, DbExecutionLoad load = default, params DbArg[] args);
        Task<T?> ScalarAsync<T>(string sql, DbExecutionLoad load = default , params DbArg[] args);
        Task<DbRow?> RowAsync(string sql, DbExecutionLoad load = default, params DbArg[] args);
        Task<DbRows> RowsAsync(string sql, DbExecutionLoad load = default, params DbArg[] args);
    }
}
