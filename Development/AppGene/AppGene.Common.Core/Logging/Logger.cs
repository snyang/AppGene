using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Core.Logging
{
    internal class Logger : ILogger
    {
        private NLog.Logger logger;

        internal Logger(string name)
        {
            logger = new NLog.LogFactory().GetLogger(name);
        }

        public bool IsDebugEnabled
        {
            get
            {
                return logger.IsDebugEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return logger.IsErrorEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return logger.IsInfoEnabled;
            }
        }

        public bool IsTraceEnabled
        {
            get
            {
                return logger.IsTraceEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return logger.IsWarnEnabled;
            }
        }

        public string Name
        {
            get
            {
                return logger.Name;
            }
        }

        public void Debug(Exception exception)
        {
            logger.Debug(exception);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug(Exception exception, string message)
        {
            logger.Debug(exception, message);
        }

        public void Debug(string format, params object[] args)
        {
            logger.Debug(CultureInfo.InvariantCulture, format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            logger.Debug(exception, CultureInfo.InvariantCulture, format, args);
        }

        public void Error(Exception exception)
        {
            logger.Error(exception);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception exception, string message)
        {
            logger.Error(exception, message);
        }

        public void Error(string format, params object[] args)
        {
            logger.Error(CultureInfo.InvariantCulture, format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            logger.Error(exception, CultureInfo.InvariantCulture, format, args);
        }

        public void Info(Exception exception)
        {
            logger.Info(exception);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(Exception exception, string message)
        {
            logger.Info(exception, message);
        }

        public void Info(string format, params object[] args)
        {
            logger.Info(CultureInfo.InvariantCulture, format, args);
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            logger.Info(exception, CultureInfo.InvariantCulture, format, args);
        }

        public void Trace(Exception exception)
        {
            logger.Trace(exception);
        }

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Trace(Exception exception, string message)
        {
            logger.Trace(exception, message);
        }

        public void Trace(string format, params object[] args)
        {
            logger.Trace(CultureInfo.InvariantCulture, format, args);
        }

        public void Trace(Exception exception, string format, params object[] args)
        {
            logger.Trace(exception, CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(Exception exception)
        {
            logger.Warn(exception);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn(Exception exception, string message)
        {
            logger.Warn(exception, message);
        }

        public void Warn(string format, params object[] args)
        {
            logger.Warn(CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            logger.Warn(exception, CultureInfo.InvariantCulture, format, args);
        }
    }
}
