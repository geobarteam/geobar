using System;

namespace GoSimple.Messaging {

	/// <summary>
	/// Defines the common properties that all <see cref="ISignal{T}"/> instances will have.
	/// A signal is analogous to an event.
	/// </summary>
	/// <typeparam name="T">The event body type</typeparam>
	public interface ISignal<out T> {

        /// <summary>
        /// The unique signal id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The correlation id
        /// </summary>
        string CorrelationId { get; }

		/// <summary>
		/// Gets the time when the message was sent (for the first time)
		/// </summary>
		DateTime Timestamp { get; }

		/// <summary>
		/// Gets the name of the sending application
		/// </summary>
		string Sender { get; }

		/// <summary>
		/// Gets the body or payload of this instance.
		/// </summary>
		T Body { get; }

		/// <summary>
		/// Gets the context where this signal was created, e.g. STO, FLX, LNG, ...
		/// </summary>
		/// <value>The context.</value>
		string Context { get;}

		/// <summary>
		/// Gets whether or not the message is being redelivered (has been delivered before).
		/// </summary>
		/// <value>True if the message was delivered before, false otherwise</value>
		bool Redelivered { get; }

	}

}
