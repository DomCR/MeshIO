using System;

namespace MeshIO.Formats;

/// <summary>
/// Defines a contract for reading scenes from a data source and receiving notifications during the reading process.
/// </summary>
/// <remarks>Implementations of this interface are responsible for managing the underlying data source and
/// providing scene data sequentially. The interface inherits from <see cref="IDisposable"/>, so callers should ensure
/// that resources are released appropriately by calling <see cref="IDisposable.Dispose"/> when finished.</remarks>
public interface ISceneReader : IDisposable
{
	/// <summary>
	/// Occurs when a notification is raised by the component.
	/// </summary>
	/// <remarks>Subscribe to this event to receive notifications as they occur. The event provides information
	/// about the notification through the associated event arguments.</remarks>
	public event NotificationEventHandler OnNotification;

	/// <summary>
	/// Reads and returns the next scene from the underlying data source.
	/// </summary>
	/// <returns>A <see cref="Scene"/> object representing the next scene. Returns <see langword="null"/> if there are no more
	/// scenes to read.</returns>
	public Scene Read();
}