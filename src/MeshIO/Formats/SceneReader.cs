using CSUtilities.IO;
using System;
using System.IO;

namespace MeshIO.Formats
{
	/// <summary>
	/// Provides an abstract base class for reading 3D scene data from various file formats. Supports notification events
	/// and resource management for derived scene reader implementations.
	/// </summary>
	/// <remarks>SceneReader offers factory methods to create appropriate scene reader instances based on file
	/// extension or format type. It manages the underlying data stream and exposes a notification event for reporting
	/// progress or issues during scene reading. Derived classes implement the specific logic for parsing different 3D file
	/// formats. This class is not thread-safe.</remarks>
	public abstract class SceneReader : ISceneReader
	{
		/// <inheritdoc/>
		public event NotificationEventHandler OnNotification;

		internal readonly StreamIO _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="SceneReader"/> class for reading from the specified file path.
		/// </summary>
		/// <param name="path">The path to the file to be read. Must refer to an existing file accessible for reading.</param>
		/// <param name="notification">An optional delegate to receive notifications during the reading process. May be null if no notifications are
		/// required.</param>
		protected SceneReader(string path, NotificationEventHandler notification = null)
			: this(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), notification)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SceneReader"/> class using the specified stream and optional notification handler.
		/// </summary>
		/// <param name="stream">The input stream containing scene data. The stream must support seeking and cannot be null.</param>
		/// <param name="notification">An optional delegate to handle notification events during scene reading. May be null if no notifications are
		/// required.</param>
		/// <exception cref="ArgumentNullException">Thrown if the stream parameter is null.</exception>
		/// <exception cref="ArgumentException">Thrown if the stream does not support seeking.</exception>
		protected SceneReader(Stream stream, NotificationEventHandler notification = null)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking");

			this._stream = new StreamIO(stream);
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
}