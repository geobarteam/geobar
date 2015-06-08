using System;

namespace GoSimple.Messaging {

	/// <summary>
	/// Interface to receive <see cref="IMessage{T}"/> messages from the <see cref="IMessaging"/>.
	/// </summary>
	/// <typeparam name="T">The payload type</typeparam>
    public interface IReceiver<T> : IConsumer
    {
		/// <summary>
		/// Is invoked every time a message arrives on the specified queue.
		/// </summary>
        event MessageHandler<T> OnMessage;

		/// <summary>
		/// Commits the current session. Only use this with Transactions = false and
		/// AutoCommit = false;
		/// </summary>
		[Obsolete("This kind of operation makes no sense and will be removed. Atomicity between multiple asynchonous treatment operations is anathema to everything message-based and should be killed with fire. Failure to process a message should be expressed using an exception.")]
		void Commit();

		/// <summary>
		/// Rolls back the current session. Only use this with Transactions = false and
		/// AutoCommit = false;
        /// </summary>
        [Obsolete("This kind of operation makes no sense and will be removed. Atomicity between multiple asynchonous treatment operations is anathema to everything message-based and should be killed with fire. Failure to process a message should be expressed using an exception.")]
		void Rollback();
	}
}
