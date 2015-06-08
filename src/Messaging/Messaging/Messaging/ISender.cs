namespace GoSimple.Messaging
{
    /// <summary>
    /// Interface to send <see cref="IMessage{T}"/> messages to the <see cref="IMessaging"/>.
    /// </summary>
    /// <typeparam name="T">The event body type</typeparam>
    public interface ISender<T> : IProducer
    {
        /// <summary>
        /// Sends a message to the bus.
        /// </summary>
        /// <param name="payload">The payload to send</param>
        /// <exception cref="ConnectionException">When Tibco is down</exception>
        string Send(T payload);
    }
}
