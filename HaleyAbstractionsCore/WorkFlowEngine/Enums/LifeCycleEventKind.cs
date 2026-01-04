using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Enums {
    public enum LifeCycleEventKind {
        //lifecycle events are macro items, hook are for micro orchestration. 
        //lifecycle events are generic for a given state change.. we dont exactly know what steps should be followed.. so we just raise one event.. application should listen to this and then decide what to do. Here, the decision inside application is hard-coded.
        //hooks are exact application method that should be called. Application should merely listen to these hooks and then invoke the methods.. Preferably, we will implement a way to auto-invoke these methods using reflection.. but for now, application should listen and invoke the methods. Later, we will implement auto-invocation using reflection inside HaleyEngineItself.. Application merely has to register the methods with HaleyEngine.
        Transition = 1,
        Hook = 2
    }
}
