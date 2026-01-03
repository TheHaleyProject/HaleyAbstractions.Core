using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Enums {
    public enum LifeCycleAckOutcome { 
        Delivered = 1, 
        Processed = 2, 
        Failed = 3, 
        Retry = 4 
    }
}
