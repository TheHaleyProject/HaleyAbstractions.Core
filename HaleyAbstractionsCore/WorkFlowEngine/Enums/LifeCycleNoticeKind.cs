using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Enums {
    public enum LifeCycleNoticeKind { 
        Error = 1, 
        Warning = 2, 
        Info = 3 ,
        OverDue = 4 //When a specific instance remains in same state for too long but this is not tracked by time-out policies.. then we start sending overdue notice..
    }
}
