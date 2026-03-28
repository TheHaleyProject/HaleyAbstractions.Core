namespace Haley.Models {
    /// <summary>
    /// Result returned by ILifeCycleRuntimeBus.ImportBackfillAsync.
    /// </summary>
    public sealed class BackfillImportResult {
        public bool    Success      { get; init; }
        /// <summary>
        /// Human-readable reason on failure.
        /// Well-known values: "NotValidated", "InvalidTransition:{from}→{to}", "DefinitionNotFound".
        /// </summary>
        public string? Reason       { get; init; }
        /// <summary>GUID of the created/updated instance. Set only on success.</summary>
        public string? InstanceGuid { get; init; }

        public static BackfillImportResult Ok(string instanceGuid)
            => new() { Success = true, InstanceGuid = instanceGuid };

        public static BackfillImportResult Fail(string reason)
            => new() { Success = false, Reason = reason };
    }
}
