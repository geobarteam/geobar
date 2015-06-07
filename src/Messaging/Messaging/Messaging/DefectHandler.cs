using System;

namespace GoSimple.Messaging
{

    /// <summary>
    /// Delegate that is invoked by the <see cref="IMessageBus"/> when
    /// an error occurs.
    /// Normally, exceptions are thrown by the method that was called. This
    /// delegate is used only when an error occurs that the client should know
    /// about when no method was called, e.g. when waiting for a message.
    /// </summary>
    /// <param name="exception">The exception that occurred</param>
    public delegate void DefectHandler(Exception exception);
}
