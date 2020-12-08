using System;
using Microsoft.Extensions.Logging;


namespace LoggingWithMicrosoft
{
    public class LoggerFactoryAdapter : Inetlab.SMPP.Logging.ILogFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Inetlab.SMPP.Logging.LogLevel _minLevel;

        public LoggerFactoryAdapter(ILoggerFactory loggerFactory): this(loggerFactory,  Inetlab.SMPP.Logging.LogLevel.Info)
        {

        }

        public LoggerFactoryAdapter(ILoggerFactory loggerFactory,  Inetlab.SMPP.Logging.LogLevel minLevel)
        {
            _loggerFactory = loggerFactory;
            _minLevel = minLevel;
        }

        public Inetlab.SMPP.Logging.ILog GetLogger(string name)
        {
            return new LoggerFactoryLogger(_loggerFactory.CreateLogger(name), _minLevel);
        }


    }

    public class LoggerFactoryLogger : Inetlab.SMPP.Logging.ILog
    {
        private readonly ILogger _logger;
        private readonly Inetlab.SMPP.Logging.LogLevel _minLevel;


        public LoggerFactoryLogger(ILogger logger, Inetlab.SMPP.Logging.LogLevel minLevel)
        {
            _logger = logger;
            _minLevel = minLevel;
        }


        private Microsoft.Extensions.Logging.LogLevel GetLevel(Inetlab.SMPP.Logging.LogLevel level)
        {
            switch (level)
            {

                case Inetlab.SMPP.Logging.LogLevel.Verbose:
                    return global::Microsoft.Extensions.Logging.LogLevel.Trace;
                case Inetlab.SMPP.Logging.LogLevel.Debug:
                    return global::Microsoft.Extensions.Logging.LogLevel.Debug;
                case Inetlab.SMPP.Logging.LogLevel.Info:
                    return global::Microsoft.Extensions.Logging.LogLevel.Information;
                case Inetlab.SMPP.Logging.LogLevel.Warning:
                    return global::Microsoft.Extensions.Logging.LogLevel.Warning;
                case Inetlab.SMPP.Logging.LogLevel.Error:
                    return global::Microsoft.Extensions.Logging.LogLevel.Error;
                case Inetlab.SMPP.Logging.LogLevel.Fatal:
                    return global::Microsoft.Extensions.Logging.LogLevel.Critical;
            }

            return Microsoft.Extensions.Logging.LogLevel.None;
        }

        public bool IsEnabled(Inetlab.SMPP.Logging.LogLevel level)
        {
            return level >= _minLevel && _logger.IsEnabled(GetLevel(level));
        }

        public void Write(Inetlab.SMPP.Logging.LogLevel level, string message, Exception exception = null, params object[] values)
        {
            if (level < _minLevel) return;

            _logger.Log(GetLevel(level), exception, message, values);
        }

    }


}