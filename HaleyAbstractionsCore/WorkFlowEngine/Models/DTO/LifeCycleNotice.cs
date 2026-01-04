using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public sealed class LifeCycleNotice {
        public LifeCycleNoticeKind Kind { get; }
        public string NoticeType { get; }          // machine-readable category (string)
        public string Code { get; }                // short code (stable)
        public string Message { get; }             // human readable
        public Exception? Exception { get; }
        public DateTimeOffset OccurredAt { get; }
        public IReadOnlyDictionary<string, object?>? Data { get; }  // optional structured payload

        public LifeCycleNotice(LifeCycleNoticeKind kind, string noticeType, string code, string message, Exception? exception = null, IReadOnlyDictionary<string, object?>? data = null) {
            Kind = kind;
            NoticeType = noticeType ?? string.Empty;
            Code = code ?? string.Empty;
            Message = message ?? string.Empty;
            Exception = exception;
            Data = data;
            OccurredAt = DateTimeOffset.UtcNow;
        }

        public static LifeCycleNotice Error(string noticeType, string code, string message, Exception ex, IReadOnlyDictionary<string, object?>? data = null) => new LifeCycleNotice(LifeCycleNoticeKind.Error, noticeType, code, message, ex, data);
        public static LifeCycleNotice Warn(string noticeType, string code, string message, IReadOnlyDictionary<string, object?>? data = null) => new LifeCycleNotice(LifeCycleNoticeKind.Warning, noticeType, code, message, null, data);
        public static LifeCycleNotice Info(string noticeType, string code, string message, IReadOnlyDictionary<string, object?>? data = null) => new LifeCycleNotice(LifeCycleNoticeKind.Info, noticeType, code, message, null, data);
    }
}
