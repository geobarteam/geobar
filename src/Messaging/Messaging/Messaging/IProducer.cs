using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSimple.Messaging
{
    public interface IProducer : IDisposable
    {
        /// <summary>
        /// Commits the current session. Only use this with Transactions = false and
        /// AutoCommit = false;
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back the current session. Only use this with Transactions = false and
        /// AutoCommit = false;
        /// </summary>
        void Rollback();

        /// <summary>
        /// Closes this instance, sessions and producers.
        /// </summary>
        void Close();

        string Destination { get; }
    }
}
