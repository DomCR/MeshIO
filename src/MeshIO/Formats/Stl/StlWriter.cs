using MeshIO.Entities.Geometries;
using System.Collections.Generic;
using System.IO;

namespace MeshIO.Formats.Stl
{
	/// <summary>
	/// Provides functionality to write 3D scenes or meshes to files or streams in the STL (Stereolithography) format.
	/// </summary>
	/// <remarks>The StlWriter class supports both ASCII and binary STL output, as determined by the specified
	/// options. It can write single or multiple meshes, or an entire scene, to a file or stream. Notification events can be
	/// used to receive progress or status updates during the write operation. This class is not thread-safe.</remarks>
	public class StlWriter : SceneWriter<StlWriterOptions>
	{
		/// <inheritdoc/>
		public StlWriter(string path, Scene scene, StlWriterOptions options = null, NotificationEventHandler notification = null)
			: base(path, scene, options, notification) { }

		/// <inheritdoc/>
		public StlWriter(Stream stream, Scene scene, StlWriterOptions options = null, NotificationEventHandler notification = null)
			: base(stream, scene, options, notification)
		{
		}

		/// <summary>
		/// Writes the specified mesh to a file in STL format at the given path.
		/// </summary>
		/// <param name="path">The file system path where the STL file will be written. If the file exists, it will be overwritten.</param>
		/// <param name="mesh">The mesh to write to the STL file. Cannot be null.</param>
		/// <param name="options">Optional settings that control how the STL file is written. If null, default options are used.</param>
		/// <param name="notification">An optional event handler for receiving progress or status notifications during the write operation. If null, no
		/// notifications are sent.</param>
		public static void WriteMesh(string path, Mesh mesh, StlWriterOptions options = null, NotificationEventHandler notification = null)
		{
			WriteMesh(path, [mesh], options, notification);
		}

		/// <summary>
		/// Writes one or more meshes to a file in STL format at the specified path.
		/// </summary>
		/// <param name="path">The file system path where the STL file will be created. If the file already exists, it will be overwritten.</param>
		/// <param name="meshes">A collection of meshes to write to the STL file. Each mesh in the collection will be included in the output.</param>
		/// <param name="options">Optional settings that control how the STL file is written. If null, default options are used.</param>
		/// <param name="notification">An optional event handler that receives progress or status notifications during the write operation. Can be null
		/// if notifications are not needed.</param>
		public static void WriteMesh(string path, IEnumerable<Mesh> meshes, StlWriterOptions options = null, NotificationEventHandler notification = null)
		{
			WriteMesh(File.Create(path), meshes, options, notification);
		}

		/// <summary>
		/// Writes the specified mesh to the provided stream in STL format.
		/// </summary>
		/// <param name="stream">The output stream to which the mesh data will be written. The stream must be writable.</param>
		/// <param name="mesh">The mesh to write to the stream. Cannot be null.</param>
		/// <param name="options">Optional settings that control how the mesh is written. If null, default options are used.</param>
		/// <param name="notification">An optional event handler for receiving notifications or progress updates during the write operation. Can be null
		/// if notifications are not required.</param>
		public static void WriteMesh(Stream stream, Mesh mesh, StlWriterOptions options = null, NotificationEventHandler notification = null)
		{
			WriteMesh(stream, [mesh], options, notification);
		}

		/// <summary>
		/// Writes one or more meshes to the specified stream in STL format using the provided options.
		/// </summary>
		/// <remarks>The method supports both binary and ASCII STL formats, as specified by the options. The caller is
		/// responsible for managing the lifetime of the provided stream. If the stream is not writable, an exception may be
		/// thrown by the underlying writer.</remarks>
		/// <param name="stream">The output stream to which the STL data will be written. The stream must be writable and remain open for the
		/// duration of the operation.</param>
		/// <param name="meshes">A collection of meshes to write to the stream. Each mesh represents a 3D object to be exported.</param>
		/// <param name="options">The options that control how the STL data is written, such as content type (binary or ASCII). If null, default
		/// options are used.</param>
		/// <param name="notification">An optional event handler that receives notifications about the writing process. Can be null if no notifications
		/// are needed.</param>
		public static void WriteMesh(Stream stream, IEnumerable<Mesh> meshes, StlWriterOptions options = null, NotificationEventHandler notification = null)
		{
			if (options == null)
			{
				options = new StlWriterOptions();
			}

			IStlStreamWriter writer = null;
			switch (options.ContentType)
			{
				case ContentType.Binary:
					writer = new StlBinaryStreamWriter(stream, meshes);
					break;
				case ContentType.ASCII:
					writer = new StlTextStreamWriter(stream, meshes, options);
					break;
			}

			writer.OnNotification += notification;
			writer.Write();
		}

		/// <inheritdoc/>
		public override void Write()
		{
			IStlStreamWriter writer = null;
			switch (this.Options.ContentType)
			{
				case ContentType.Binary:
					writer = new StlBinaryStreamWriter(_stream, _scene);
					break;
				case ContentType.ASCII:
					writer = new StlTextStreamWriter(_stream, _scene, Options);
					break;
			}

			writer.OnNotification += this.onNotificationEvent;
			writer.Write();
		}
	}
}