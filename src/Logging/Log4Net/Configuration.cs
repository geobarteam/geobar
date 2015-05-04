using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Logging.Log4Net
{
    public class Configuration
    {
        private static FileInfo log4NetConfigurationFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));

        /// <summary>
        /// Gets or sets a value indicating whether to enable internal debugging. 
        /// This is disabled by default.
        /// </summary>
        /// <value>
        /// 	<c>True</c> if internal debugging must be enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool InternalDebuggingEnabled { get; set; }

        /// <summary>
        /// Gets or set the path to the log4net configuration file.
        /// </summary>
        public static string Log4NetConfigurationFile
        {
            get
            {
                if (!Configuration.log4NetConfigurationFile.Exists)
                {
                    throw new ArgumentException(String.Format((string)"The specified log4net configuration file cannot be found at {0}.", Configuration.log4NetConfigurationFile.FullName));
                }

                return Configuration.log4NetConfigurationFile.FullName;
            }

            set
            {
                FileInfo configFile;
                if (value == null)
                {
                    configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
                }
                else
                {
                    configFile = Path.IsPathRooted(value) ? new FileInfo(value) : new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value));
                }

                if (!configFile.Exists)
                {
                    throw new ArgumentException(String.Format("The specified log4net configuration file cannot be found at {0}.", configFile.FullName));
                }

                Configuration.log4NetConfigurationFile = configFile;
            }
        }
    }
}
