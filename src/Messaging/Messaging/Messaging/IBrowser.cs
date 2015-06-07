using System.Collections.Generic;
using GoSimple.Messaging;

namespace GoSimple.Messaging
{
    /// <summary>
    /// Interface to view <see cref="IMessage{T}"/> instances for a destination.
    /// </summary>
    /// <remarks>
    ///	Tibco specific: a Tibco destination with an explicit message expiration 
    /// defined is not browsable.
    /// </remarks>
    public interface IBrowser<T>
    {
        /// <summary>
        /// Returns all messages currently in the destination.
        /// </summary>
        IEnumerable<Message<T>> Messages { get; }
    }
}
