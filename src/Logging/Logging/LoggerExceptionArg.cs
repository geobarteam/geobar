using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoBar.Framework.Logging
{
    public class LoggerExceptionArg : EventArgs
    {
        private readonly string _eventSourceName;
        private readonly LogLevel _logLevel;

        public LoggerExceptionArg(string eventSourceName, LogLevel logLevel)
        {
            _eventSourceName = eventSourceName;
            _logLevel = logLevel;
        }

        public LogLevel LogLevel
        {
            get { return _logLevel; }
        }

        public string EventSourceName
        {
            get { return _eventSourceName; }
        }
    }
}
