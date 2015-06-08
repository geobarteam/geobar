namespace GoSimple.Messaging
{

    public interface IMessagingFactory
    {
        /// <summary>
        /// Returns an <see cref="IReceiver{T}"/> instance that can be used to receive <see cref="IMessage{T}"/>
        /// messages from the <see cref="IMessaging"/>.
        /// </summary>
        /// <typeparam name="T">The message body type</typeparam>
        /// <param name="destination">The name of the destination (i.e. queue or topic)</param>
        /// <returns>An <see cref="IReceiver{T}"/> instance</returns>
        IReceiver<T> CreateReceiver<T>(string destination);

        /// <summary>
        /// Creates a temporary receiver.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IReceiver<T> CreateReceiver<T>();

        /// <summary>
        /// Returns an <see cref="ISender{T}"/> instance that can be used to send <see cref="IMessage{T}"/>
        /// messages to the <see cref="IMessaging"/>.
        /// </summary>
        /// <typeparam name="T">The message body type</typeparam>
        /// <param name="destination">The name of the destination (i.e. queue or topic)</param>
        /// <returns>An <see cref="ISender{T}"/> instance</returns>
        ISender<T> CreateSender<T>(string destination);

        /// <summary>
        /// Returns an <see cref="IPublisher{T}"/> instance that can be used to send <see cref="ISignal{T}"/>
        /// events to the <see cref="IMessaging"/>.
        /// </summary>
        /// <typeparam name="T">The event body type</typeparam>
        /// <param name="destination">The name of the destination (i.e. queue or topic)</param>
        /// <returns>An <see cref="IPublisher{T}"/> instance</returns>
        IPublisher<T> CreatePublisher<T>(string destination);

        /// <summary>
        /// Returns an <see cref="ISubscriber{T}"/> instance that can be used to receive <see cref="ISignal{T}"/>
        /// events from the <see cref="IMessaging"/>.
        /// </summary>
        /// <typeparam name="T">The event body type</typeparam>
        /// <param name="destination">The name of the destination (i.e. queue or topic)</param>
        /// <returns>An <see cref="ISubscriber{T}"/> instance</returns>
        ISubscriber<T> CreateSubscriber<T>(string destination);

        /// <summary>
        /// Returns an <see cref="ISubscriber{T}"/> instance that can be used to receive <see cref="ISignal{T}"/>
        /// events from the <see cref="IMessaging"/> in a durable fashion.
        /// A durable subscription lives on the server. A durable subscriber can use this subscription
        /// to receive events that were generated when it (the subscriber) was offline. This contrasts with a
        /// normal subscriber, who only receives events generated when it was subscribed.
        /// </summary>
        /// <typeparam name="T">The event body type</typeparam>
        /// <param name="destination">The name of the destination (i.e. queue or topic)</param>
        /// <param name="subscription">The name of the durable subscription - A subscriber will receive the message only once based on this name.</param>
        /// <returns>An <see cref="ISubscriber{T}"/> instance</returns>
        ISubscriber<T> CreateDurableSubscriber<T>(string destination, string subscription);

        /// <summary>
        /// Instantiates a <see cref="IBrowser{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        IBrowser<T> CreateBrowser<T>(string destination);

        /// <summary>
        /// The current Session
        /// </summary>
        ISession Session { get; }
    }
}

