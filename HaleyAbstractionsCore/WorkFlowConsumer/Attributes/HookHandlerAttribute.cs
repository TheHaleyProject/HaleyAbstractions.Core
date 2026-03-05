namespace Haley.Abstractions {
    /// <summary>
    /// Marks a method on a <see cref="Haley.Abstractions.LifeCycleWrapper"/> as the handler
    /// for a specific hook route.
    /// The method must have signature: <c>Task&lt;AckOutcome&gt; Method(ILifeCycleHookEvent, ConsumerContext)</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HookHandlerAttribute : Attribute {
        /// <summary>The route name this method handles (e.g. "pam.work.outbox").</summary>
        public string Route { get; }
        /// <summary>
        /// Minimum handler version required for this method to be selected.
        /// The dispatcher picks the method with the highest MinVersion that is &lt;= the pinned handler_version.
        /// Use 0 (default) to match any version.
        /// </summary>
        public int MinVersion { get; }
        public HookHandlerAttribute(string route, int minVersion = 0) {
            if (string.IsNullOrWhiteSpace(route)) throw new ArgumentNullException(nameof(route));
            Route = route;
            MinVersion = minVersion;
        }
    }
}
