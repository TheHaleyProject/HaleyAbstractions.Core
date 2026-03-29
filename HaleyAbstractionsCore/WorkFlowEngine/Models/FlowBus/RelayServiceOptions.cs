namespace Haley.Models {
    /// <summary>
    /// Configuration for WorkflowRelayHost — typically bound from appsettings.
    /// AssemblyPrefixes filters which loaded assemblies are scanned for WorkflowRelayBase subclasses.
    /// When empty, all loaded assemblies are scanned (convenient but slow in large apps).
    /// Example: ["MyApp", "WFE"] scans only assemblies whose name starts with "MyApp" or "WFE".
    /// </summary>
    public sealed class RelayServiceOptions {
        public List<string> AssemblyPrefixes { get; set; } = new();
    }
}
