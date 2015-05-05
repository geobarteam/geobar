using System;
using log4net.Appender;
using log4net.Core;

namespace GoSimple.Logging.Log4Net.Appenders
{
    public class UdpSyslogAppender : RemoteSyslogAppender, ISyslogAppender
    {
        public string CorrelationProperty { get; set; }
        private readonly SyslogFormatter syslogFormatter;

        public UdpSyslogAppender()
        {
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
                byte[] bytes = base.Encoding.GetBytes(text.ToCharArray());
                base.Client.Send(bytes, bytes.Length, base.RemoteEndPoint);
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

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.syslogFormatter.ActivateOptions();
        }
    }
}
