using MeshIO.Core;
using MeshIO.FBX.Readers;
using MeshIO.FBX.Readers.Parsers;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxReader : ReaderBase
	{
		public FbxReaderOptions Options { get; } = new FbxReaderOptions();

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
		/// Read the FBX file
		/// </summary>
		public Scene Read()
		{
			FbxRootNode root;
			using (IFbxParser parser = getParser(this._stream, this.Options))
			{
				root = parser.Parse();
			}

			var reader = FbxFileBuilderBase.Create(root, this.Options);
			reader.OnNotification += this.onNotificationEvent;

			return reader.Read();
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

		private static IFbxParser getParser(Stream stream, FbxReaderOptions options)
		{
			IFbxParser parser = null;
			if (FbxBinary.ReadHeader(stream))
			{
				parser = new FbxBinaryParser(stream, options.ErrorLevel);
			}
			else
			{
				parser = new FbxAsciiParser(stream, options.ErrorLevel);
			}

			return parser;
		}
	}
}
