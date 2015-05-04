using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace GoSimple.Logging.Log4Net
{
    public class Log4NetAdapter : LogImpl, ILog
    {
        public Log4NetAdapter(ILogger logger)
            : base(logger)
        {
        }

        public void Log(string source, LogLevel level, string message, string correlationId = null)
        {
            var log4NetEvent = new LoggingEvent(typeof (Log4NetAdapter), Logger.Repository, Logger.Name, MapLevel(level), message,
                null);
            Logger.Log(log4NetEvent);
        }

        private static Level MapLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.All:
                    return Level.All;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Emergency:
                    return Level.Emergency;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warning:
                    return Level.Warn;
            }

            throw new ArgumentException(String.Format("{0} level is not supported!", level), "level");
        }
    }
}
