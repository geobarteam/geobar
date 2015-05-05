using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace GoSimple.Logging.Log4Net.Appenders
{
    public class TcpAppender : AppenderSkeleton
    {
        private DateTime lastConnectionAttempt;
        private DateTime lastErrorTime;
        private int remotePort;
        private int nbSubsequentErrors;

        private bool isConnectionBroken;

        public TcpAppender()
        {
            Encoding = Encoding.Default;
            ConnectionAttemptInterval = 10000;
            ConnectionAttemptIntervalAfterErrors = 600000;
            NbErrorsBeforeBreakingConnection = 3;
            TcpWriteTimeout = 500;
        }

        public IPAddress RemoteAddress { get; set; }
        public int ConnectionAttemptInterval { get; set; }
        public int ConnectionAttemptIntervalAfterErrors { get; set; }
        public int NbErrorsBeforeBreakingConnection { get; set; }

        public int RemotePort
        {
            get
            {
                return this.remotePort;
            }
            set
            {
                if (value < 0 || value > (int)ushort.MaxValue)
                {
                    throw new ArgumentException(string.Format("The value specified is less than {0}  or greater than {1}.", 0, ushort.MaxValue));
                }

                this.remotePort = value;
            }
        }

        public int TcpWriteTimeout { get; set; }

        public Encoding Encoding { get; set; }

        protected TcpClient Client { get; set; }

        protected IPEndPoint RemoteEndPoint { get; set; }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.RemoteAddress == null)
                throw new ArgumentException("The required property 'RemoteAddress' was not specified.");

            if (this.RemotePort < 0 || this.RemotePort > (int)ushort.MaxValue)
            {
                throw new ArgumentException(string.Format("The RemotePort specified is less than {0}  or greater than {1}.", 0, ushort.MaxValue));
            }

            this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
            this.InitializeClientConnection();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                Send(this.RenderLoggingEvent(loggingEvent));
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error(string.Format("Unable to send logging event to remote host {0} on port {1}.", this.RemoteAddress, this.RemotePort), ex, ErrorCode.WriteFailure);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Client == null)
                return;
            this.Client.Close();
            this.Client = null;
        }

        protected virtual void InitializeClientConnection()
        {
            try
            {
                this.Client = new TcpClient();
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error(string.Format("Could not initialize the TcpClient connection on address {0} on port {1}.", this.RemoteAddress, this.RemotePort), ex, ErrorCode.GenericFailure);
                this.Client = null;
            }
        }

        protected virtual void Send(string content)
        {
            try
            {
                if (!IsConnectionBroken())
                {
                    if (!this.Client.Connected)
                    {
                        if (CanReconnect)
                        {
                            lastConnectionAttempt = DateTime.Now;
                            this.Client.Connect(this.RemoteAddress, this.RemotePort);
                        }
                        else
                        {
                            this.ErrorHandler.Error(string.Format("Unable to send logging event to remote host because last connection was attempted less than {0}ms ago.", ConnectionAttemptInterval), null, ErrorCode.WriteFailure);
                        }
                    }

                    CoreSend(content);
                }
            }
            catch (IOException)
            {
                //Attempt reconnect
                if (CanReconnect)
                {
                    lastConnectionAttempt = DateTime.Now;
                    nbSubsequentErrors = 0;
                    this.Client.Close();
                    this.Client = null;
                    this.InitializeClientConnection();
                    this.Client.Connect(this.RemoteAddress, this.RemotePort);
                    CoreSend(content);
                }
                else
                {
                    this.ErrorHandler.Error(string.Format("Unable to send logging event to remote host because last connection was attempted less than {0}ms ago.", ConnectionAttemptInterval), null, ErrorCode.WriteFailure);
                }
            }
        }

        private bool IsConnectionBroken()
        {
            if (isConnectionBroken)
            {
                if (DateTime.Now - lastErrorTime > TimeSpan.FromMilliseconds(ConnectionAttemptIntervalAfterErrors))
                {
                    isConnectionBroken = false;
                    nbSubsequentErrors = 0;
                }
            }
            return isConnectionBroken;
        }

        private bool CanReconnect
        {
            get
            {
                return DateTime.Now - lastConnectionAttempt > TimeSpan.FromMilliseconds(ConnectionAttemptInterval);
            }
        }

        private void CoreSend(string content)
        {
            try
            {
                if (this.Client.Connected)
                {
                    byte[] bytes = this.Encoding.GetBytes(content.ToCharArray());
                    this.Client.GetStream().WriteTimeout = TcpWriteTimeout;
                    this.Client.GetStream().Write(bytes, 0, bytes.Length);
                    nbSubsequentErrors = 0;
                }
            }
            catch
            {
                nbSubsequentErrors++;
                lastErrorTime = DateTime.Now;

                if (nbSubsequentErrors >= NbErrorsBeforeBreakingConnection)
                {
                    isConnectionBroken = true;
                    EventLog.WriteEntry(
                        string.Format(
                            "Multiple consecutive errors occured when sending to the syslog server. Stop sending for {0} milliseconds",
                            ConnectionAttemptIntervalAfterErrors),
                        "logging",
                        EventLogEntryType.Error);
                }

                throw;
            }
        }
    }
}
