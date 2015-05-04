using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace GoSimple.Logging.Log4Net
{
    public class LogFactory
    {
        /// <summary>
        /// Obtain a Logger for the <see cref="ILog"/> per EventSourceName.
        /// </summary>
        /// <param name="eventSourceName">The name of the eventSource</param>
        /// <returns>the <see cref="ILog"/> instance for a specified eventSourceName</returns>
        public static Log4NetAdapter Obtain(string eventSourceName)
        {
            
            if (string.IsNullOrEmpty(eventSourceName)) throw new ArgumentNullException("eventSourceName");

            Log4NetAdapter adapter = LogFactory.GetLog4NetAdapter(eventSourceName);
            if (adapter == null)
            {
                throw new ArgumentException("Could not obtain a Log4Net Logger for '" + eventSourceName + "'", "eventSourceName");
            }

            adapter.Logger.Repository.GetAppenders()
                .Where(appender => appender is FileAppender).ToList()
                .ForEach(appender => ((FileAppender) appender).Encoding = Encoding.UTF8);
           
            return adapter;
        }
        
        private static Log4NetAdapter GetLog4NetAdapter(string name)
        {
            return UnwrapLog4NetAdapter(Assembly.GetCallingAssembly(), name);
        }

        private static Log4NetAdapter UnwrapLog4NetAdapter(Assembly assembly, string name)
        {
            return WrapLogger(LoggerManager.GetLogger(assembly, name));
        }

        private static Log4NetAdapter WrapLogger(ILogger logger)
        {
            return (Log4NetAdapter) WrapperMap.GetWrapper(logger);
        }

        private static readonly WrapperMap WrapperMap = new WrapperMap(WrapperCreationHandler);

        private static ILoggerWrapper WrapperCreationHandler(log4net.Core.ILogger logger)
        {
            return new Log4NetAdapter(logger);
        }
    }
}
