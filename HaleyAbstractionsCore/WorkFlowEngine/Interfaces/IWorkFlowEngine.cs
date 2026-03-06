using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions {
    public interface IWorkFlowEngine : ILifeCycleEngine, IAsyncDisposable {
        event Func<ILifeCycleEvent, Task>? EventRaised;
        event Func<LifeCycleNotice, Task>? NoticeRaised;
    }
}
