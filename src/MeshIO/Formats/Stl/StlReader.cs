using MeshIO.Entities.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.Formats.Stl
{
	/// <summary>
	/// Provides functionality to read 3D geometry data from STL (Stereolithography) files and streams, supporting both
	/// binary and ASCII STL formats.
	/// </summary>
	/// <remarks>The StlReader class enables importing mesh data from STL files into a scene graph. It supports
	/// reading from file paths or streams and can notify callers of progress or events during the reading process via an
	/// optional notification handler. STL files are commonly used for 3D printing and CAD applications.</remarks>
	public class StlReader : SceneReader
	{
		/// <summary>
		/// Initializes a new instance of the StlReader class for reading STL files from the specified path.
		/// </summary>
		/// <param name="path">The file system path to the STL file to be read. Cannot be null or empty.</param>
		/// <param name="notification">An optional delegate to receive notifications or progress updates during the reading process. Can be null if
		/// notifications are not required.</param>
		public StlReader(string path, NotificationEventHandler notification = null)
			: base(path, notification)
		{
		}

		/// <summary>
		/// Initializes a new instance of the StlReader class using the specified input stream.
		/// </summary>
		/// <param name="stream">The input stream containing the STL data to be read. The stream must be readable and positioned at the beginning of
		/// the STL content.</param>
		/// <param name="notification">An optional delegate to receive notifications or progress updates during the reading process. Can be null if
		/// notifications are not required.</param>
		public StlReader(Stream stream, NotificationEventHandler notification = null)
			: base(stream, notification)
		{
		}

		/// <summary>
		/// Determines the content type of a mesh stream by inspecting its header.
		/// </summary>
		/// <remarks>The method reads the first five bytes of the stream to identify the content type. The stream
		/// position is reset to its original position after the operation. The method assumes the stream contains data in a
		/// supported mesh format.</remarks>
		/// <param name="stream">The input stream containing mesh data to analyze. The stream must be readable and seekable.</param>
		/// <returns>A value indicating whether the mesh data in the stream is in ASCII or binary format.</returns>
		/// <exception cref="NullReferenceException">Thrown if <paramref name="stream"/> is null.</exception>
		public static MeshIO.Formats.ContentType GetContentType(Stream stream)
		{
			if (stream == null)
			{
				throw new NullReferenceException(nameof(stream));
			}

			if (stream.Length < 5)
			{
				return ContentType.Binary;
			}

			byte[] buffer = new byte[5];
			string header;

			stream.Seek(0, SeekOrigin.Begin);
			stream.Read(buffer, 0, buffer.Length);
			stream.Seek(0, SeekOrigin.Begin);

			header = Encoding.ASCII.GetString(buffer);

			return StlFileToken.Solid.Equals(header, StringComparison.InvariantCultureIgnoreCase) ? ContentType.ASCII : ContentType.Binary;
		}

		/// <inheritdoc/>
		public override Scene Read()
		{
			Scene scene = new Scene();

			var meshes = this.ReadMeshes();

			scene.RootNode.Entities.AddRange(meshes);

			return scene;
		}

		/// <summary>
		/// Read the STL file
		/// </summary>
		/// <returns><see cref="Mesh"/> defined in the file</returns>
		public IEnumerable<Mesh> ReadMeshes()
		{
			this._stream.Position = 0;

			IStlStreamReader reader = null;
			var contentType = StlReader.GetContentType(this._stream.Stream);
			switch (contentType)
			{
				case ContentType.Binary:
					reader = new StlBinaryStreamReader(this._stream.Stream);
					break;
				case ContentType.ASCII:
					reader = new StlTextStreamReader(this._stream.Stream);
					break;
			}

			reader.OnNotification += this.onNotificationEvent;

			return reader.Read();
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			this._stream.Dispose();
		}
	}
}
