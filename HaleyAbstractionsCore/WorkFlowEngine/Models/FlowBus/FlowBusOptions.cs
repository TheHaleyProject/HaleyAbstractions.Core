using Haley.Enums;

namespace Haley.Models {
    /// <summary>
    /// Configuration for FlowBus — typically bound from appsettings.
    /// Mode null means auto-resolve: Engine ?? Relay ?? throw.
    /// </summary>
    public sealed class FlowBusOptions {
        public FlowBusMode? Mode { get; set; } //Default mode.
    }
}
