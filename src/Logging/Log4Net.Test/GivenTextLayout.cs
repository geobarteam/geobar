using GoSimple.Logging;
using GoSimple.Logging.Log4Net;

namespace Log4Net.Test
{
    public class GivenTextLayout : GivenWhenThen
    {
        public const string LogFilePath = "log.txt"; //should also be set in config file!!!!

        public LoggerExceptionEventArg LoggerExceptionEvent { get; set; }

        public override void Given()
        {
            base.Given();
            Configuration.Log4NetConfigurationFile = "TextLayout.config";
            Logger.ExceptionHandler += Logger_ExceptionHandler;
            Logger.Initialize(new Log4NetLogger());
        }

        public void Logger_ExceptionHandler(object sender, LoggerExceptionEventArg e)
        {
            this.LoggerExceptionEvent = e;
        }
    }
}