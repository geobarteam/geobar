using System.Text.RegularExpressions;
using log4net.Core;
using log4net.Layout;
using System.IO;
using System;
using System.Threading;

namespace GoSimple.Logging.Log4Net
{
    public class TextLayout : LayoutSkeleton
    {
        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            // check arguments
            if (loggingEvent == null) return;
            if (loggingEvent.MessageObject == null && loggingEvent.RenderedMessage == null) return;

            // get logger id
            string loggerId = string.Empty;
            if (loggingEvent.Properties != null && loggingEvent.Properties.Contains("__objectId") && loggingEvent.Properties["__objectId"] != null)
            {
                loggerId = loggingEvent.Properties["__objectId"].ToString();
            }

            // prepare stuff
            string message = loggingEvent.MessageObject == null ? loggingEvent.RenderedMessage : loggingEvent.MessageObject.ToString();
            string[] lines = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string header = string.Format("{0} [{1}] ({3}|{4}) {2} : ", loggingEvent.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss,fff"), loggingEvent.Level.DisplayName.PadLeft(5, ' '), ClassNameFormatter.Format(loggingEvent.LoggerName), Thread.CurrentThread.GetHashCode().ToString().PadLeft(2, ' '), loggerId.PadLeft(2));
            const string filler = "\t";

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    writer.Write(header);
                }
                else
                {
                    writer.Write(filler);
                }
                writer.WriteLine(lines[i]);
            }

        }

        public override void ActivateOptions()
        {

        }
    }

    /// <summary>
    /// Utility class, used for formatting class names into a human readable format.
    /// </summary>
    internal static class ClassNameFormatter
    {
        private static readonly Regex genericPattern = new Regex(@"(?<LOGGER>[^`]+)(`(.*)\[+(?<GENERIC>[^,*]+),*(.*)\]+)");

        /// <summary>
        /// Returns a formatted string representation of the provided class name.
        /// </summary>
        /// <param name="fullClassName">Full name of the class to be formatted.</param>
        /// <example>
        /// Fluxys.Vega.Infrastructure.Repository`1[Fluxys.Vega.Domain.ForecastGroup]
        /// 
        /// will result in:
        /// 
        /// Repository<ForecastGroup>
        /// </example>
        public static string Format(string fullClassName)
        {
            Match m = genericPattern.Match(fullClassName);
            if (m.Success)
            {
                if (m.Groups["GENERIC"].Value.Length != 0)
                {
                    return (RemoveNamespace(m.Groups["LOGGER"].Value) + "<" + RemoveNamespace(m.Groups["GENERIC"].Value) + ">");
                }
                else
                {
                    return (RemoveNamespace(m.Groups["LOGGER"].Value));
                }
            }
            else
            {
                return (RemoveNamespace(fullClassName));
            }
        }

        /// <summary>
        /// Strips the namespace from the provided class name.
        /// </summary>
        /// <param name="fullClassName">The full name of the class of which the namespace should be stripped.</param>
        public static string RemoveNamespace(string fullClassName)
        {
            if (fullClassName == null) return (null);
            int pos = fullClassName.LastIndexOf(".");
            if (pos >= 0 && pos < (fullClassName.Length - 1))
            {
                return (fullClassName.Substring(pos + 1, fullClassName.Length - 1 - pos));
            }
            return (fullClassName);
        }
    }
}

