using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
   public sealed class LifeCycleNotice {
    public LifeCycleNoticeKind Kind { get; }
    public string Code { get; }
    public string Message { get; }
    public Exception? Exception { get; }
    public DateTimeOffset OccurredAt { get; }

    public LifeCycleNotice(LifeCycleNoticeKind kind, string code, string message, Exception? exception = null) {
        Kind = kind;
        Code = code ?? string.Empty;
        Message = message ?? string.Empty;
        Exception = exception;
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public static LifeCycleNotice Error(string code, string message, Exception ex) => new LifeCycleNotice(LifeCycleNoticeKind.Error, code, message, ex);
    public static LifeCycleNotice Warn(string code, string message) => new LifeCycleNotice(LifeCycleNoticeKind.Warning, code, message);
    public static LifeCycleNotice Info(string code, string message) => new LifeCycleNotice(LifeCycleNoticeKind.Info, code, message);
}
}
