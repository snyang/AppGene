using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Core.Logging
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsTraceEnabled { get; }
        bool IsWarnEnabled { get; }
        string Name { get; }
        void Debug(string message);

        void Debug(string format, params object[] args);

        void Debug(Exception exception);

        void Debug(Exception exception, string message);

        void Debug(Exception exception, string format, params object[] args);

        void Error(string message);

        void Error(string format, params object[] args);

        void Error(Exception exception);

        void Error(Exception exception, string message);

        void Error(Exception exception, string format, params object[] args);

        void Info(string message);
        void Info(string format, params object[] args);
        void Info(Exception exception);
        void Info(Exception exception, string message);
        void Info(Exception exception, string format, params object[] args);
        void Trace(string message);
        void Trace(string format, params object[] args);
        void Trace(Exception exception);
        void Trace(Exception exception, string message);
        void Trace(Exception exception, string format, params object[] args);
        void Warn(string message);
        void Warn(string format, params object[] args);
        void Warn(Exception exception);
        void Warn(Exception exception, string message);
        void Warn(Exception exception, string format, params object[] args);
    }
}
