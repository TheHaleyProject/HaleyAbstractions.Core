using Haley.Models;
using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleHookEvent : LifeCycleEvent, ILifeCycleHookEvent {
        public LifeCycleEventKind Kind { get { return LifeCycleEventKind.Hook; } }
        public long HookId { get; set; }
        public bool OnEntry { get; set; }
        public string HookCode { get; set; } //Route or identifier for the hook
        public DateTimeOffset? NotBefore { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public LifeCycleHookEvent() { }
        public LifeCycleHookEvent(LifeCycleEvent evt) :base(evt){
            
        }
    }
}