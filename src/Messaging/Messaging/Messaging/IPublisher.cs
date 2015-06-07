namespace GoSimple.Messaging
{
    /// <summary>
    /// Interface to publish events to the <see cref="IMessageBus"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPublisher<T> : IProducer
    {
        
        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <param name="signal">The message.</param>
        /// <returns></returns>
        string Publish(Message<T> message);
    }
}
