using System;
using System.Globalization;
using System.IO;
using System.Threading;
using GoSimple.Logging.Log4Net.Appenders;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace GoSimple.Logging.Log4Net
{
    /// <summary>
    /// This formatter creates proper syslog messages following the RFC5424 spec
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc5424"/>
    /// <example>
    /// The syslog message has the following ABNF [RFC5234] definition:
    /// 
    /// SYSLOG-MSG      = HEADER SP STRUCTURED-DATA [SP MSG]
    /// 
    /// HEADER          = PRI VERSION SP TIMESTAMP SP HOSTNAME
    ///                   SP APP-NAME SP PROCID SP MSGID
    /// PRI             = "&lt;" PRIVAL "&gt;"
    /// PRIVAL          = 1*3DIGIT ; range 0 .. 191
    /// VERSION         = NONZERO-DIGIT 0*2DIGIT
    /// HOSTNAME        = NILVALUE / 1*255PRINTUSASCII
    /// 
    /// APP-NAME        = NILVALUE / 1*48PRINTUSASCII
    /// PROCID          = NILVALUE / 1*128PRINTUSASCII
    /// MSGID           = NILVALUE / 1*32PRINTUSASCII
    /// 
    /// TIMESTAMP       = NILVALUE / FULL-DATE "T" FULL-TIME
    /// FULL-DATE       = DATE-FULLYEAR "-" DATE-MONTH "-" DATE-MDAY
    /// DATE-FULLYEAR   = 4DIGIT
    /// DATE-MONTH      = 2DIGIT  ; 01-12
    /// DATE-MDAY       = 2DIGIT  ; 01-28, 01-29, 01-30, 01-31 based on
    ///                           ; month/year
    /// FULL-TIME       = PARTIAL-TIME TIME-OFFSET
    /// PARTIAL-TIME    = TIME-HOUR ":" TIME-MINUTE ":" TIME-SECOND
    ///                   [TIME-SECFRAC]
    /// TIME-HOUR       = 2DIGIT  ; 00-23
    /// TIME-MINUTE     = 2DIGIT  ; 00-59
    /// TIME-SECOND     = 2DIGIT  ; 00-59
    /// TIME-SECFRAC    = "." 1*6DIGIT
    /// TIME-OFFSET     = "Z" / TIME-NUMOFFSET
    /// TIME-NUMOFFSET  = ("+" / "-") TIME-HOUR ":" TIME-MINUTE
    /// 
    /// 
    /// STRUCTURED-DATA = NILVALUE / 1*SD-ELEMENT
    /// SD-ELEMENT      = "[" SD-ID *(SP SD-PARAM) "]"
    /// SD-PARAM        = PARAM-NAME "=" %d34 PARAM-VALUE %d34
    /// SD-ID           = SD-NAME
    /// PARAM-NAME      = SD-NAME
    /// PARAM-VALUE     = UTF-8-STRING ; characters '"', '\' and
    ///                                ; ']' MUST be escaped.
    /// SD-NAME         = 1*32PRINTUSASCII
    ///                   ; except '=', SP, ']', %d34 (")
    /// 
    /// MSG             = MSG-ANY / MSG-UTF8
    /// MSG-ANY         = *OCTET ; not starting with BOM
    /// MSG-UTF8        = BOM UTF-8-STRING
    /// BOM             = %xEF.BB.BF
    /// 
    /// 
    /// UTF-8-STRING    = *OCTET ; UTF-8 string as specified
    ///                   ; in RFC 3629
    /// 
    /// OCTET           = %d00-255
    /// SP              = %d32
    /// PRINTUSASCII    = %d33-126
    /// NONZERO-DIGIT   = %d49-57
    /// DIGIT           = %d48 / NONZERO-DIGIT
    /// NILVALUE        = "-"
    /// </example>
    /// <example>
    /// &lt;165&gt;1 2003-10-11T22:14:15.003Z mymachine.example.com evntslog - ID47 [exampleSDID@32473 iut="3" eventSource="Application" eventID="1011"] BOMAn application event log entry...
    /// </example>
    internal class SyslogFormatter
    {
        private readonly LevelMapping levelMapping = new LevelMapping();
        private readonly ISyslogAppender appender;

        public SyslogFormatter(ISyslogAppender appender)
        {
            this.appender = appender;
        }

        public string FormatEvent(LoggingEvent loggingEvent, Action<TextWriter, LoggingEvent> baseRender)
        {
            var properties = loggingEvent.Properties;

            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                int value = GeneratePriority(appender.Facility, this.GetSeverity(loggingEvent.Level));

                // PRI
                stringWriter.Write('<');
                stringWriter.Write(value);
                stringWriter.Write('>');

                // VERSION
                stringWriter.Write("1 ");

                // TIMESTAMP
                stringWriter.Write(loggingEvent.TimeStamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"));
                stringWriter.Write(' ');

                // HOSTNAME
                stringWriter.Write(Environment.MachineName);
                stringWriter.Write(' ');

                // APP-NAME
                if (appender.Identity != null)
                {
                    using (var identityWriter = new StringWriter(CultureInfo.InvariantCulture))
                    {
                        appender.Identity.Format(identityWriter, loggingEvent);
                        stringWriter.Write(identityWriter.ToString().Replace(" ", string.Empty));
                    }
                }
                else
                {
                    stringWriter.Write('-');
                }
                stringWriter.Write(' ');

                // PROCID
                if (!string.IsNullOrWhiteSpace(appender.CorrelationProperty) &&
                    properties.Contains(appender.CorrelationProperty) &&
                    properties[appender.CorrelationProperty] != null &&
                    !string.IsNullOrWhiteSpace(properties[appender.CorrelationProperty].ToString()))
                {
                    stringWriter.Write(properties[appender.CorrelationProperty]);
                }
                else
                {
                    stringWriter.Write('-');
                }
                stringWriter.Write(' ');

                // MSGID
                stringWriter.Write("- ");

                // STRUCTURED-DATA
                stringWriter.Write("[event logger=\"{0}\" thread=\"{1}\" username=\"{2}\"]", loggingEvent.LoggerName, Thread.CurrentThread.GetHashCode(), loggingEvent.UserName);
                if (properties.Count > 0)
                {
                    stringWriter.Write("[custom");

                    foreach (var key in properties.GetKeys())
                    {
                        stringWriter.Write(" {0}=\"{1}\"", key, properties[key]);
                    }

                    stringWriter.Write(']');
                }
                stringWriter.Write(' ');

                baseRender(stringWriter, loggingEvent);

                return stringWriter.ToString();
            }
        }

        protected virtual RemoteSyslogAppender.SyslogSeverity GetSeverity(Level level)
        {
            RemoteSyslogAppender.LevelSeverity levelSeverity = this.levelMapping.Lookup(level) as RemoteSyslogAppender.LevelSeverity;
            if (levelSeverity != null)
                return levelSeverity.Severity;
            if (level >= Level.Alert)
                return RemoteSyslogAppender.SyslogSeverity.Alert;
            if (level >= Level.Critical)
                return RemoteSyslogAppender.SyslogSeverity.Critical;
            if (level >= Level.Error)
                return RemoteSyslogAppender.SyslogSeverity.Error;
            if (level >= Level.Warn)
                return RemoteSyslogAppender.SyslogSeverity.Warning;
            if (level >= Level.Notice)
                return RemoteSyslogAppender.SyslogSeverity.Notice;
            if (level >= Level.Info)
                return RemoteSyslogAppender.SyslogSeverity.Informational;
            else
                return RemoteSyslogAppender.SyslogSeverity.Debug;
        }

        public static int GeneratePriority(RemoteSyslogAppender.SyslogFacility facility, RemoteSyslogAppender.SyslogSeverity severity)
        {
            if (facility < RemoteSyslogAppender.SyslogFacility.Kernel || facility > RemoteSyslogAppender.SyslogFacility.Local7)
                throw new ArgumentException("SyslogFacility out of range", "facility");
            if (severity < RemoteSyslogAppender.SyslogSeverity.Emergency || severity > RemoteSyslogAppender.SyslogSeverity.Debug)
                throw new ArgumentException("SyslogSeverity out of range", "severity");
            else
                return (int)((int)facility * 8 + severity);
        }

        public void ActivateOptions()
        {
            this.levelMapping.ActivateOptions();
        }
    }
}

