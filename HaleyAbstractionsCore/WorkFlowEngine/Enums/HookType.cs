namespace Haley.Enums {
    /// <summary>
    /// Classifies a hook by its execution role.
    /// Gate   (1) — was blocking=true.  Validates a condition; failure terminates the chain immediately.
    /// Effect (0) — was blocking=false. Side effect (email, log, audit); result is always ignored.
    /// </summary>
    public enum HookType {
        Effect = 0,
        Gate   = 1,
    }
}
