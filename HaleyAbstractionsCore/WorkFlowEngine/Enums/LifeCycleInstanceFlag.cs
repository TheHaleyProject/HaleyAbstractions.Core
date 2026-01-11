using System;

namespace Haley.Enums {

    [Flags]
    public enum LifeCycleInstanceFlag : int {
        None = 0,
        Active = 1 << 0,
        Suspended = 1 << 1, //application can mark this instnace as suspended (may be it doens't track this anymore)
        Completed = 1 << 2,
        Failed = 1 << 3, //application can mark this failed, because this instnace is deleted in app side.
        Archived = 1 << 4
    }
}