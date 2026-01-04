using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Enums {
    public enum AckOutcome {
        //Just the application input.. Application sends this to the engine to indicate the outcome of processing an event.
        Delivered = 1, 
        Processed = 2, 
        Failed = 3, 
        Retry = 4 
    }
}
