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
	public abstract class SceneReader : IDisposable, ISceneReader
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

		/// <summary>
		/// Creates an appropriate scene reader for the specified file path based on the file's extension.
		/// </summary>
		/// <remarks>Supported file formats include FBX, GLB, GLTF, OBJ, and STL. The method selects the appropriate
		/// reader implementation based on the file extension. If the file format is not supported, a <see
		/// cref="NotSupportedException"/> is thrown.</remarks>
		/// <param name="path">The path to the scene file to be read. The file extension determines the reader type. Cannot be null or empty.</param>
		/// <param name="notification">An optional event handler for receiving notifications during the reading process. May be null if no notifications
		/// are required.</param>
		/// <returns>An <see cref="ISceneReader"/> instance capable of reading the specified file format.</returns>
		/// <exception cref="NotSupportedException">Thrown if the file extension is not recognized or supported.</exception>
		public static ISceneReader GetReader(string path, NotificationEventHandler notification = null)
		{
			var type = FileExtensions.FromExtension(Path.GetExtension(path));
			switch (type)
			{
				case FileFormatType.Fbx:
					return new FbxReader(path, notification);
				case FileFormatType.Glb:
				case FileFormatType.Gltf:
					return new GltfReader(path, notification);
				case FileFormatType.Obj:
					return new Obj.ObjReader(path, notification);
				case FileFormatType.Stl:
					return new Stl.StlReader(path, notification);
				case FileFormatType.Unknown:
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Creates an appropriate scene reader for the specified file format and input stream.
		/// </summary>
		/// <param name="type">The file format type to read from the stream. Must be a supported value of <see cref="FileFormatType"/>.</param>
		/// <param name="stream">The input stream containing the scene data to be read. The stream must be readable and positioned at the start of
		/// the data.</param>
		/// <param name="notification">An optional event handler for receiving notifications during the reading process. May be <see langword="null"/> if
		/// notifications are not required.</param>
		/// <returns>An <see cref="ISceneReader"/> instance capable of reading scene data from the specified stream in the given
		/// format.</returns>
		/// <exception cref="NotSupportedException">Thrown if <paramref name="type"/> is <see cref="FileFormatType.Unknown"/> or not supported.</exception>
		public static ISceneReader GetReader(FileFormatType type, Stream stream, NotificationEventHandler notification = null)
		{
			switch (type)
			{
				case FileFormatType.Fbx:
					return new FbxReader(stream, notification);
				case FileFormatType.Glb:
				case FileFormatType.Gltf:
					return new GltfReader(stream, notification);
				case FileFormatType.Obj:
					return new Obj.ObjReader(stream, notification);
				case FileFormatType.Stl:
					return new Stl.StlReader(stream, notification);
				case FileFormatType.Unknown:
				default:
					throw new NotSupportedException();
			}
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