using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoSimple.Logging
{
        /// <summary>
        /// Main entry point
        /// </summary>
        public static class Logger
        {
            private static readonly object syncRoot = new object();

            private static ILog loggerImp;

            private static ILog LoggerImp
            {
                get
                {
                    if (loggerImp == null)
                    {
                        throw new ApplicationException("Initialize should be called first!");
                    }
                    return loggerImp;
                }
            }

            private static string defaultPropertiesFormatter(IDictionary<string, object> properties)
            {
                if (properties != null && properties.Any())
                {
                    var argumentsBuilder = new StringBuilder();
                    properties.Keys.ToList().ForEach(key =>
                    {
                        string value = properties[key] != null
                            ? string.Format("{0}={1}", key,
                                properties[key].ToString())
                            : "null";
                        argumentsBuilder.Append(string.Format("{0}={1}", key, value));
                    });
                    argumentsBuilder.Append(Environment.NewLine);
                    
                    return (argumentsBuilder.ToString());
                }

                return string.Empty;
            }

            private static string defaultExceptionFormatter(Exception ex)
            {
                var bldr = new StringBuilder();
                while (ex != null)
                {
                    bldr.Append("Type: ");
                    bldr.Append(ex.GetType().ToString());
                    bldr.Append(Environment.NewLine);
                    bldr.Append("Message: ");
                    bldr.Append(ex.Message);
                    bldr.Append(Environment.NewLine);
                    bldr.Append("Stack trace: ");
                    bldr.Append(Environment.NewLine);
                    bldr.Append(ex.StackTrace);
                    ex = ex.InnerException;
                    if (ex != null)
                    {
                        bldr.Append(Environment.NewLine);
                        bldr.Append("Inner exception:");
                        bldr.Append(Environment.NewLine);
                    }
                }
                return (bldr.ToString());
            }



            private static Func<IDictionary<string, object>, string> FormatProperties { get; set; } 

            private static Func<Exception, string> FormatException { get; set; }

            public static event EventHandler<LoggerExceptionEventArg> ExceptionHandler;

            public static event EventHandler<EmergencyEventArg> EmergencyHandler;

            /// <summary>
            /// Initialize the Logger
            /// </summary>
            /// <param name="logger">The logger.</param>
            public static void Initialize(ILog logImplemantation, InitializationConfiguration configuration = null)
            {
                lock (syncRoot)
                {
                    loggerImp = logImplemantation;
                    if (configuration != null)
                    {
                        FormatProperties = configuration.PropertiesFormatter ?? defaultPropertiesFormatter;
                        FormatException = configuration.ExceptionFormatter ?? defaultExceptionFormatter;    
                    }
                    else
                    {
                        FormatProperties = defaultPropertiesFormatter;
                        FormatException = defaultExceptionFormatter; 
                    }
                }
            }

            /// <summary>
            /// Deduces an event source name from an object: if the object is actually a type, 
            /// the type name is returned, otherwise the name of the object's type.
            /// </summary>
            /// <param name="sender"></param>
            /// <returns></returns>
            private static string EventSourceName(object sender)
            {
                if (sender == null) return ("Application");
                if (sender is String) return sender as String;
                if (sender is Type) return (((Type) sender).FullName);

                return (sender.GetType().FullName);
            }

            public static void Log(object sender, string message, LogLevel logLevel, IDictionary<string, object> customProps = null)
            {
                try
                {
                    if (customProps != null)
                    {
                        message = message + Environment.NewLine + FormatProperties(customProps);
                    }

                    LoggerImp.Log(EventSourceName(sender), logLevel, message);
                }
                catch (Exception e)
                {
                    if (ExceptionHandler != null)
                    {
                        ExceptionHandler(e, new LoggerExceptionEventArg(EventSourceName(sender), logLevel));
                    }
                }
            }
            
            public static void Emergency(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.Emergency, customProps);
                OnEmergency(sender, message, customProps);
            }

            public static void Emergency(object sender, Exception ex, IDictionary<string, object> customProps = null)
            {
                Log(sender, FormatException(ex), LogLevel.Emergency, customProps);
                OnEmergency(sender, FormatException(ex),customProps);
            }

            public static void Error(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.Error, customProps);
            }
            
            public static void Error(object sender,  Exception ex, IDictionary<string, object> customProps = null)
            {
                Log(sender, FormatException(ex), LogLevel.Error, customProps);
            }

            public static void Warn(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.Warning, customProps);
            }

            public static void Info(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.Info, customProps);
            }
            
            public static void Debug(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.Debug, customProps);
            }

            public static void All(object sender, string message, IDictionary<string, object> customProps = null)
            {
                Log(sender, message, LogLevel.All, customProps);
            }

            private static void OnEmergency(object sender, string message, IDictionary<string, object> customProps)
            {
                if (EmergencyHandler != null)
                {
                    if (customProps != null)
                    {
                        message = message + Environment.NewLine + FormatProperties(customProps);
                    }

                    if (message != null)
                    {
                        EmergencyHandler(sender, new EmergencyEventArg(sender, message));
                    }
                }
            }
        } 
}
