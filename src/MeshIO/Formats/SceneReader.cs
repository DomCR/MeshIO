using CSUtilities.IO;
using System;
using System.IO;

namespace MeshIO.Formats;

/// <summary>
/// Provides an abstract base class for reading 3D scene data from various file formats. Supports notification events
/// and resource management for derived scene reader implementations.
/// </summary>
/// <remarks>SceneReader offers factory methods to create appropriate scene reader instances based on file
/// extension or format type. It manages the underlying data stream and exposes a notification event for reporting
/// progress or issues during scene reading. Derived classes implement the specific logic for parsing different 3D file
/// formats. This class is not thread-safe.</remarks>
public abstract class SceneReader<T> : ISceneReader
	where T : SceneReaderOptions, new()
{
	/// <inheritdoc/>
	public event NotificationEventHandler OnNotification;

	public T Options
	{
		get => this._options;
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), $"The SceneReader options cannot be null.");
			}

			this._options = value;
		}
	}

	internal readonly StreamIO _stream;

	private T _options;

	/// <summary>
	/// Initializes a new instance of the <see cref="SceneReader{T}"/> class using the specified file path, options, and optional
	/// notification handler.
	/// </summary>
	/// <param name="path">The path to the scene file to open for reading. Must refer to an existing file.</param>
	/// <param name="options">The options used to configure the scene reading operation.</param>
	/// <param name="notification">An optional delegate to receive notifications during the reading process. May be null if notifications are not
	/// required.</param>
	protected SceneReader(string path, T options, NotificationEventHandler notification = null)
		: this(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), options, notification)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SceneReader{T}"/> class using the specified stream, options, and optional notification
	/// handler.
	/// </summary>
	/// <param name="stream">The input stream containing the scene data to be read. The stream must support seeking and remain open for the
	/// duration of the SceneReader instance.</param>
	/// <param name="options">The options used to configure the scene reading process. If null, a default instance of the options type is used.</param>
	/// <param name="notification">An optional event handler that receives notifications during the scene reading process. May be null if no
	/// notifications are required.</param>
	/// <exception cref="ArgumentNullException">Thrown if stream is null.</exception>
	/// <exception cref="ArgumentException">Thrown if stream does not support seeking.</exception>
	protected SceneReader(Stream stream, T options, NotificationEventHandler notification = null)
	{
		if (stream == null)
			throw new ArgumentNullException(nameof(stream));

		if (!stream.CanSeek)
			throw new ArgumentException("The stream must support seeking");

		this._stream = new StreamIO(stream);
		this._options = options ?? new T();
		this.OnNotification += notification;
	}

	/// <inheritdoc/>
	public virtual void Dispose()
	{
		this._stream.Dispose();
	}

	/// <inheritdoc/>
	public abstract Scene Read();

	protected void onNotificationEvent(object sender, NotificationEventArgs e)
	{
		this.OnNotification?.Invoke(this, e);
	}

	protected void triggerNotification(string message, NotificationType notificationType, Exception ex = null)
	{
		this.onNotificationEvent(this, new NotificationEventArgs(message, notificationType, ex));
	}
}