namespace Haley.Abstractions {
    /// <summary>
    /// Marks a <see cref="LifeCycleWrapper"/> subclass as the handler for a specific workflow
    /// definition. The consumer library auto-discovers decorated classes at startup and resolves
    /// the definition ID from the engine using <see cref="Name"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LifeCycleDefinitionAttribute : Attribute {
        /// <summary>The workflow definition name as registered in the engine.</summary>
        public string Name { get; }
        public LifeCycleDefinitionAttribute(string name) {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }
    }
}
