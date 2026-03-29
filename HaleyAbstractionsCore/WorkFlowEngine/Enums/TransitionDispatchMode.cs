namespace Haley.Enums {
    public enum TransitionDispatchMode {
        NormalRun      = 0,  // No hooks — run business logic + auto-transition as usual.
        ValidationMode = 1,  // Has hooks — run business logic, ACK result, do NOT auto-transition.
    }
}
