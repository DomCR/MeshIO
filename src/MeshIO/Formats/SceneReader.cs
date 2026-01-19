using CSUtilities.IO;
using MeshIO.Formats.Fbx;
using MeshIO.Formats.Gltf;
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

		protected SceneReader(string path, NotificationEventHandler notification = null)
			: this(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), notification)
		{
		}

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