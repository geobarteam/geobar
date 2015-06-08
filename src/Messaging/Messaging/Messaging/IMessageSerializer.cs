using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;

namespace GoSimple.Messaging
{
    /// <summary>
    /// Required interface for any serializer implementation to be usable
    /// by the <see cref="IMessaging"/>.
    /// </summary>
    public interface IMessageSerializer<T>
    {
        /// <summary>
        /// Serializes the specified message
        /// </summary>
        /// <param name="pm">The pm.</param>
        /// <param name="bodyType">Type of the body.</param>
        /// <returns></returns>
        string Serialize(Message<T> message);

        /// <summary>
        /// Deserializes the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="bodyType">Type of the body.</param>
        /// <returns></returns>
        Message<T> Deserialize(string content);
    }
}
