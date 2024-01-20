using MeshIO.Core;
using MeshIO.FBX.Converters;
using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxReader : ReaderBase
	{
		private Stream _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		public FbxReader(string path) : this(new FileStream(path, FileMode.Open)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public FbxReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this._stream = stream;
		}

		/// <summary>
		/// Get the fbx version of the file
		/// </summary>
		/// <returns></returns>
		public FbxVersion GetVersion()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Read the FBX file
		/// </summary>
		public Scene Read()
		{
			using(FbxFileReaderBase reader = FbxFileReaderBase.Create(this._stream))
			{
				reader.Read();
			}
		
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			_stream.Dispose();
		}

		/// <summary>
		/// Read a fbx file into an scene
		/// </summary>
		/// <returns></returns>
		public static Scene Read(string path, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(path))
			{
				reader.OnNotification += notificationHandler;
				return reader.Read();
			}
		}

		/// <summary>
		/// Read a fbx stream into an scene
		/// </summary>
		/// <returns></returns>
		public static Scene Read(Stream stream, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(stream))
			{
				reader.OnNotification += notificationHandler;
				return reader.Read();
			}
		}
	}
}
