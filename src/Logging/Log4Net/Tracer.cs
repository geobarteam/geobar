using System;
using System.Diagnostics;
using System.Reflection;

namespace GoSimple.Logging.Log4Net
{
    /// <summary>
    /// Traces messages to the console, but only when <see cref="LoggerConfiguration.InternalDebuggingEnabled"/> is enabled.
    /// </summary>
    public static class Tracer
    {
        private static readonly Type tracerType = typeof(Tracer);

        /// <summary>
        /// Debugging/tracing statements.
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            if (Configuration.InternalDebuggingEnabled)
            {
                Console.WriteLine(EventSourceName(GetCaller()) + ": " + message);
            }
        }

        /// <summary>
        /// Returns the calling method, i.e. the first method that does not belong to the
        /// Logger class.
        /// </summary>
        /// <returns></returns>
        private static string GetCaller()
        {
            StackTrace t = new StackTrace();

            for (int frameCounter = 0; frameCounter < t.FrameCount; frameCounter++)
            {
                MethodBase method = t.GetFrame(frameCounter).GetMethod();
                if (method.ReflectedType != tracerType)
                {
                    return method.ReflectedType.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// Deduces an event source name from an object.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns>
        /// if the object is null, the name of the Application event log,
        /// if the object is a string, the string,
        /// if the object is actually a type, the type's FullName
        /// otherwise the name of the object's type.
        /// </returns>
        private static string EventSourceName(object sender)
        {
            if (sender == null)
            {
                return "Application";
            }

            if (sender is string)
            {
                return sender.ToString();
            }

            if (sender is Type)
            {
                return ((Type)sender).FullName;
            }

            return sender.GetType().FullName;
        }
    }
}
