using System;

namespace MeshIO.Formats;

/// <summary>
/// Defines a contract for writing scene data and receiving notifications during the write process.
/// </summary>
/// <remarks>Implementations of this interface are responsible for persisting or exporting scene information.
/// The interface supports notification events to inform subscribers of progress or issues during the write operation.
/// Implementers should ensure proper resource management by disposing of any unmanaged resources when no longer
/// needed.</remarks>
public interface ISceneWriter : IDisposable
{
	/// <summary>
	/// Occurs when a notification is raised by the component.
	/// </summary>
	/// <remarks>Subscribers can handle this event to respond to notifications as they occur. The event provides
	/// information about the notification through the associated event arguments. This event is typically used to implement
	/// custom logic when specific notifications are triggered.</remarks>
	public event NotificationEventHandler OnNotification;

	/// <summary>
	/// Writes data to the underlying output stream or destination.
	/// </summary>
	public void Write();
}