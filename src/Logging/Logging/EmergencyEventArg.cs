using System;

namespace GoSimple.Logging
{
    public class EmergencyEventArg : EventArgs
    {
        private readonly object _sender;
        private readonly string _message;

        public EmergencyEventArg(object sender, string message)
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