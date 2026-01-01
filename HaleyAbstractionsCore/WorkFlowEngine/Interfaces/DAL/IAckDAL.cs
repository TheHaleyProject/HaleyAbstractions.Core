using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IAckDAL {
        Task<long> UpsertByConsumerAndSourceReturnIdAsync(long consumer, long source, int ackStatus, CancellationToken ct = default);
        Task<int> SetStatusAsync(long ackId, int ackStatus, CancellationToken ct = default);
        Task<int> MarkRetryAsync(long ackId, CancellationToken ct = default);
    }
}
