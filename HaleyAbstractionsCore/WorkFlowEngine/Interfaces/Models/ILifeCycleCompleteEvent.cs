namespace Haley.Abstractions {
    /// <summary>
    /// Final engine-to-consumer handoff after transition + hook orchestration is resolved.
    /// NextEvent is the engine's single suggested next step. A value of 0 means the engine
    /// has no suggestion and the consumer may decide what to do next.
    /// </summary>
    public interface ILifeCycleCompleteEvent : ILifeCycleEvent {
        long LifeCycleId { get; }
        bool HooksSucceeded { get; }
        int NextEvent { get; }
    }
}
