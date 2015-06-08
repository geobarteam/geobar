using System;

namespace GoSimple.Messaging {

	/// <summary>
	/// Delegate that is invoked by the <see cref="IMessaging"/> when a message
	/// is received.
	/// </summary>
	/// <typeparam name="T">The message body type</typeparam>
    /// <param name="message">The message</param>
	public delegate void MessageHandler<T>(Message<T> message);

}
