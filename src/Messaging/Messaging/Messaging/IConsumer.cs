using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSimple.Messaging
{
    public interface IConsumer : IDisposable
    {
        /// <summary>
        /// Is raised when an error occurs receiving a message, e.g. deserialization fails.
        /// </summary>
        event DefectHandler OnDefect;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="ConnectionException">If Tibco is down.</exception>
        void Start();

        /// <summary>
        /// Stops this instance: no more message will be received.
        /// </summary>
        void Stop();

    }
}
