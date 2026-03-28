using Haley.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    /// <summary>
    /// Implemented by the consuming application to supply legacy data during backfill graph traversal.
    ///
    /// The DefinitionWalker owns graph traversal — it walks the definition snapshot and calls
    /// these callbacks at each node. The consumer only needs to answer two questions per node:
    ///   1. Did this entity fire this event? (GetTransitionDataAsync)
    ///   2. Did this hook fire during that event? (GetHookDataAsync)
    ///
    /// The consumer works entirely in terms of event codes and hook routes — never state names.
    /// Event codes are the stable contract; state names are an engine-internal graph concern.
    /// </summary>
    public interface IBackfillDataProvider {
        /// <summary>
        /// Return transition data if the entity fired this event in its legacy history,
        /// or null if the entity never reached this event (walk stops here).
        /// </summary>
        Task<BackfillStateData?> GetTransitionDataAsync(int eventCode, CancellationToken ct = default);

        /// <summary>
        /// Return hook data if the legacy system tracked this hook firing,
        /// or null if not tracked (hook is skipped with a warning — not a hard failure).
        /// </summary>
        Task<BackfillHookData?> GetHookDataAsync(int eventCode, string route, CancellationToken ct = default);
    }
}
