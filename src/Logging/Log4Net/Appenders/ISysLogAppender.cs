using log4net.Appender;
using log4net.Layout;

namespace GoSimple.Logging.Log4Net.Appenders
{
    public interface ISyslogAppender : IAppender
    {
        string CorrelationProperty { get; set; }
        PatternLayout Identity { get; set; }
        RemoteSyslogAppender.SyslogFacility Facility { get; set; }
    }
}
