using System;
using System.Net;
using System.Text;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace GoSimple.Logging.Log4Net.Appenders
{
    public class TcpSyslogAppender : TcpAppender, ISyslogAppender
    {
        private const int DefaultSyslogPort = 514;
        private readonly SyslogFormatter syslogFormatter;

        public string CorrelationProperty { get; set; }

        public PatternLayout Identity { get; set; }
        public RemoteSyslogAppender.SyslogFacility Facility { get; set; }

        public TcpSyslogAppender()
        {
            Facility = RemoteSyslogAppender.SyslogFacility.User;
            this.RemotePort = DefaultSyslogPort;
            this.RemoteAddress = IPAddress.Parse("127.0.0.1");
            this.Encoding = Encoding.ASCII;
            syslogFormatter = new SyslogFormatter(this);
        }

        /// <summary>
        /// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)"/> method.
        /// </summary>
        /// <param name="loggingEvent">The event to log.</param>
        /// <remarks>
        /// 	<para>
        /// Writes the event to a remote syslog daemon.
        /// </para>
        /// 	<para>
        /// The format of the output will depend on the appender's layout.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                string text = syslogFormatter.FormatEvent(loggingEvent, this.RenderLoggingEvent);
                Send(text);
            }
            catch (Exception e)
            {
                this.ErrorHandler.Error(string.Concat(new object[]
				{
					"Unable to send logging event to remote syslog ",
					base.RemoteAddress.ToString(),
					" on port ",
					base.RemotePort,
					"."
				}), e, ErrorCode.WriteFailure);
            }
        }

        /// <summary>
        /// Activates the options.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.syslogFormatter.ActivateOptions();
        }
    }
}
