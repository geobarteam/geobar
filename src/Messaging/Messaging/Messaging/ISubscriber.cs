using System;

namespace GoSimple.Messaging
{
    /// <summary>
    /// Defines the interaction between an entity that want to receive events (<see cref="ISignal{T}"/>)
    /// from the <see cref="IMessageBus"/>.
    /// </summary>
    /// <typeparam name="T">The event body type</typeparam>
    public interface ISubscriber<T> : IConsumer
    {
        /// <summary>
        /// Is raized when a <see cref="ISignal{T}"/> is ready to be received.
        /// </summary>
        event SignalHandler<T> OnSignal;
    }
}
