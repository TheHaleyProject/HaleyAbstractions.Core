using System.Threading;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public interface IWorkFlowConsumerService {
        Task StartAsync(CancellationToken ct = default);
        Task StopAsync(CancellationToken ct = default);
    }
}
