using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Logging.Log4Net
{
    /// <summary>
    /// This class can be used for logging errors that occurred in the log4net-framework.
    /// If the internal debugging of Log4net is enabled, messages are witten to the console 
    /// and to the System.Diagnostics.Trace system.  This <see cref="TraceListener"/> can be
    /// used to intercept these messages and write the ERRORS to the eventlog.
    /// </summary>
    internal class Log4NetErrorTraceListener : TraceListener
    {
        /// <summary>
        /// Makes sure only log4net:ERROR messages are written to the EventLog.
        /// </summary>
        /// <param name="message">The message to be written</param>
        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        /// <summary>
        /// Makes sure only log4net:ERROR messages are written to the EventLog.
        /// </summary>
        /// <param name="message">The message to be written</param>
        public override void Write(string message)
        {
            if (message.StartsWith("log4net:ERROR"))
            {
                EventLog.WriteEntry(message, typeof(Log4NetErrorTraceListener).Name, EventLogEntryType.Error);
            }
        }
    }
}
