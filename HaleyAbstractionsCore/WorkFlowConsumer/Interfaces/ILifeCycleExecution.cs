namespace Haley.Abstractions {
    /// <summary>
    /// The engine surface exposed to workflow wrappers (<see cref="LifeCycleWrapper"/> subclasses).
    ///
    /// Combines <see cref="ILifeCycleConsumerBus"/> (register, heartbeat, ACK, blueprint import)
    /// and <see cref="ILifeCycleRuntimeBus"/> (trigger, timeline, instance data, runtime log,
    /// suspend/resume/reopen) — everything a handler needs to interact with the engine.
    ///
    /// Wrappers receive this interface and nothing more. They are deliberately shielded from
    /// <see cref="ILifeCycleEngineProxy"/>, which adds the polling methods
    /// (GetDueTransitionsAsync / GetDueHooksAsync) used exclusively by the consumer's internal
    /// dispatch loop. Exposing those to wrappers would risk accidentally draining the in-memory
    /// delivery channels and breaking the dispatch pipeline.
    ///
    /// <see cref="ILifeCycleEngineProxy"/> extends this interface, so any proxy implementation
    /// satisfies <see cref="ILifeCycleExecution"/> and can be passed directly to a wrapper.
    /// </summary>
    public interface ILifeCycleExecution : ILifeCycleConsumerBus, ILifeCycleRuntimeBus {
    }
}
