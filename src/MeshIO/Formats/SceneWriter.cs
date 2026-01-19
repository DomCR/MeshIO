using MeshIO.Formats.Fbx;
using System;
using System.IO;

namespace MeshIO.Formats;

/// <summary>
/// Provides a base class for writing scene data to a stream or file. Serves as the foundation for custom scene writer
/// implementations.
/// </summary>
/// <remarks>Derived classes should implement the Write method to define how scene data is serialized. The
/// WriterBase class manages the output stream and provides notification support via the OnNotification event. Instances
/// of WriterBase are responsible for disposing of the underlying stream when no longer needed.</remarks>
public abstract class SceneWriter<T> : ISceneWriter
	where T : SceneWriterOptions, new()
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
				throw new ArgumentNullException(nameof(value), $"The SceneWriter options cannot be null.");
			}

			this._options = value;
		}
	}

	protected readonly Scene _scene;

	protected readonly Stream _stream;

	private T _options;

	protected SceneWriter(Stream stream, Scene scene, T options = null, NotificationEventHandler notification = null)
	{
		if (stream == null)
			throw new ArgumentNullException(nameof(stream));

		if (!stream.CanSeek)
			throw new ArgumentException("The stream must support seeking.", nameof(stream));

		this._stream = stream;
		this._scene = scene;
		this._options = options ?? new T();
		this.OnNotification += notification;
	}

	protected SceneWriter(string path, Scene scene, T options = null, NotificationEventHandler notification = null)
		: this(File.Create(path), scene, options, notification)
	{
	}

		/// <inheritdoc/>
	public virtual void Dispose()
	{
		this._stream.Dispose();
	}

	/// <inheritdoc/>
	public abstract void Write();

	protected void onNotificationEvent(object sender, NotificationEventArgs e)
	{
		this.OnNotification?.Invoke(this, e);
	}

	protected void triggerNotification(string message, NotificationType notificationType, Exception ex = null)
	{
		this.onNotificationEvent(this, new NotificationEventArgs(message, notificationType, ex));
	}
}