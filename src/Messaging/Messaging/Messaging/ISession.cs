using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Messaging
{
    public interface ISession
    {
        /// <summary>
        /// Commits the current session.
        /// </summary>
        void CommitSession();

        /// <summary>
        /// Rollbacks the current session.
        /// </summary>
        void RollbackSession();

        /// <summary>
        /// The name of the application 
        /// </summary>
        string Application { get; }

        /// <summary>
        /// Is invoked when the connection is lost
        /// </summary>
        event DefectHandler OnDisconnected;

        /// <summary>
        /// Closes the current connection and creates a new one
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the configuration details of this instance.
        /// </summary>
        MessagingConfiguration Configuration { get; }

        /// <summary>
        /// A unique id that identifies this client vs the messaging infrastructure
        /// </summary>
        string ClientId { get; }
    }
}
