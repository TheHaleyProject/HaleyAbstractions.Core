namespace Haley.Abstractions {
    /// <summary>
    /// Marks a method on a <see cref="Haley.Abstractions.LifeCycleWrapper"/> as the handler
    /// for a specific transition event code.
    /// The method must have signature: <c>Task&lt;AckOutcome&gt; Method(ILifeCycleTransitionEvent, ConsumerContext)</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class TransitionHandlerAttribute : Attribute {
        /// <summary>The event code this method handles.</summary>
        public int EventCode { get; }
        /// <summary>
        /// Minimum handler version required for this method to be selected.
        /// The dispatcher picks the method with the highest MinVersion that is &lt;= the pinned handler_version.
        /// Use 0 (default) to match any version.
        /// </summary>
        public int MinVersion { get; }
        public TransitionHandlerAttribute(int eventCode, int minVersion = 0) {
            EventCode = eventCode;
            MinVersion = minVersion;
        }
    }
}
