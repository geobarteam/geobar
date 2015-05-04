using System;

namespace GoSimple.Logging
{
    public class EmergencyArg : EventArgs
    {
        private readonly object _sender;
        private readonly string _message;

        public EmergencyArg(object sender, string message)
        {
            _sender = sender;
            _message = message;
        }

        public object Sender
        {
            get { return _sender; }
        }

        public string Message
        {
            get { return _message; }
        }
    }
}