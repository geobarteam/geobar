using System;

namespace GoSimple.Messaging {

	/// <summary>
	/// Delegate that is invoked by the <see cref="IMessaging"/> when
	/// a <see cref="ISignal{T}"/> is received.
	/// </summary>
	/// <typeparam name="T">The event body type</typeparam>
    /// <param name="signal">The event</param>
	public delegate void SignalHandler<T>(ISignal<T> signal);
	
}
