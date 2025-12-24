using Haley.Enums;
using Haley.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Utils {
    public static class LifeCycleKeys {
        public static LifeCycleKey Instance(long id) => new(WorkFlowEntityKeyType.Id, id);
        public static LifeCycleKey Instance(int defVersion, string externalRef) => new(WorkFlowEntityKeyType.Composite, defVersion, externalRef);
        public static LifeCycleKey Instance(string definition, string externalRef, int environmentCode = 0) => new(WorkFlowEntityKeyType.Parent, definition, externalRef,environmentCode);

        public static LifeCycleKey Ack(string messageId) => new(WorkFlowEntityKeyType.MessageId, messageId);
        public static LifeCycleKey Ack(long transitionLogId, int consumer) => new(WorkFlowEntityKeyType.Composite, transitionLogId, consumer);

        public static LifeCycleKey Definition(long id) => new(WorkFlowEntityKeyType.Id, id);
        public static LifeCycleKey Definition(int envCode, string defName) => new(WorkFlowEntityKeyType.Composite, envCode, defName);

        public static LifeCycleKey Event(int defVersion, int code) => new(WorkFlowEntityKeyType.Composite, defVersion, code);
        public static LifeCycleKey Event(int defVersion, string name) => new(WorkFlowEntityKeyType.Composite, defVersion, name);
    }
}
