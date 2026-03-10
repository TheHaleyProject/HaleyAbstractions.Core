namespace Haley.Abstractions {
    public interface IWorkFlowEngineAccessor {
        Task<IWorkFlowEngine> GetEngineAsync(CancellationToken ct = default);
    }
}
