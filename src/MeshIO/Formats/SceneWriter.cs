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

	/// <summary>
	/// Gets or sets the options used to configure the scene writer.
	/// </summary>
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

	/// <summary>
	/// Initializes a new instance of the <see cref="SceneWriter{T}"/> class with the specified output stream, scene, options, and
	/// notification handler.
	/// </summary>
	/// <param name="stream">The output stream to which the scene data will be written. The stream must support seeking.</param>
	/// <param name="scene">The scene to be serialized and written to the output stream.</param>
	/// <param name="options">An optional options object of type T that configures the writing process. If null, a new instance of T is used.</param>
	/// <param name="notification">An optional event handler that receives notifications during the writing process.</param>
	/// <exception cref="ArgumentNullException">Thrown if stream is null.</exception>
	/// <exception cref="ArgumentException">Thrown if stream does not support seeking.</exception>
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

	/// <summary>
	/// Initializes a new instance of the <see cref="SceneWriter{T}"/> class using the specified file path, scene, options, and notification
	/// handler.
	/// </summary>
	/// <param name="path">The file system path where the scene will be written. Cannot be null or empty.</param>
	/// <param name="scene">The scene to be written to the specified file. Cannot be null.</param>
	/// <param name="options">An optional object containing settings that control how the scene is written. May be null to use default options.</param>
	/// <param name="notification">An optional delegate to receive notifications about the writing process. May be null if notifications are not
	/// required.</param>
	protected SceneWriter(string path, Scene scene, T options = null, NotificationEventHandler notification = null)
		: this(File.Create(path), scene, options, notification)
	{
	}

	/// <inheritdoc/>
	public virtual void Dispose()
	{
		this._stream.Dispose();
	}

	/// <summary>
	/// Writes data to the underlying output target. The specific behavior is defined by the derived class.
	/// </summary>
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