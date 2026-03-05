namespace Haley.Abstractions {
    /// <summary>
    /// Marks a <see cref="Haley.Abstractions.LifeCycleWrapper"/> subclass as the handler
    /// for a specific workflow definition (identified by name).
    /// Used for documentation / auto-discovery; explicit registration via
    /// <c>ConsumerService.Register&lt;T&gt;(defId)</c> is still required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LifeCycleDefinitionAttribute : Attribute {
        public string Name { get; }
        public LifeCycleDefinitionAttribute(string name) {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }
    }
}
