using System;
using Inetlab.SMPP.Logging;
using NLog;

namespace LoggingWithNLog
{
    public class NLogLoggerFactory : ILogFactory
    {
        public ILog GetLogger(string loggerName)
        {
            return new NLogLogger(loggerName);
        }
    }

    public class NLogLogger : ILog
    {
        private readonly Logger _internalLog;

        public NLogLogger(string loggerName)
        {
            _internalLog = NLog.LogManager.GetLogger(loggerName);
        }

        public bool IsEnabled(Inetlab.SMPP.Logging.LogLevel level)
        {
            return _internalLog.IsEnabled(GetLevel(level));
        }

        

        public void Write(Inetlab.SMPP.Logging.LogLevel level, string message, Exception ex, params object[] args)
        {
            _internalLog.Log(GetLevel(level), ex, message, args);
        }

        private NLog.LogLevel GetLevel(Inetlab.SMPP.Logging.LogLevel level)
        {
            switch (level)
            {
                case Inetlab.SMPP.Logging.LogLevel.All:
                case Inetlab.SMPP.Logging.LogLevel.Verbose:
                    return NLog.LogLevel.Trace;
                case Inetlab.SMPP.Logging.LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case Inetlab.SMPP.Logging.LogLevel.Info:
                    return NLog.LogLevel.Info;
                case Inetlab.SMPP.Logging.LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case Inetlab.SMPP.Logging.LogLevel.Error:
                    return NLog.LogLevel.Error;
                case Inetlab.SMPP.Logging.LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                case Inetlab.SMPP.Logging.LogLevel.Off:
                    return NLog.LogLevel.Off;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}