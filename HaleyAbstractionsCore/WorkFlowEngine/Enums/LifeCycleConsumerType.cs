using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Enums {
    public enum LifeCycleConsumerType {
        None = 0, //Common, no differentiation. 
        Transition =1,
        Hook = 2,
        Monitor = 3
    }
}
