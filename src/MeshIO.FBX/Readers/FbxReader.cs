using MeshIO.Core;
using MeshIO.FBX.Converters;
using MeshIO.FBX.Exceptions;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxReader : ReaderBase, IFbxReader
	{
		private Stream _stream;

		private ErrorLevel _errorLevel;

		private FbxRootNode _root;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		/// <param name="errorLevel">When to throw an <see cref="FbxException"/></param>
		public FbxReader(string path, ErrorLevel errorLevel)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new FileStream(path, FileMode.Open);
			_errorLevel = errorLevel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="errorLevel"></param>
		public FbxReader(Stream stream, ErrorLevel errorLevel)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = stream;
			_errorLevel = errorLevel;
		}

		/// <inheritdoc/>
		public FbxVersion GetVersion()
		{
			_root ??= this.Parse();

			return _root.Version;
		}

		/// <inheritdoc/>
		public Scene Read()
		{
			_root ??= this.Parse();
			INodeConverter converter = NodeConverterBase.GetConverter(_root, this.OnNotification);

			return converter.ConvertScene();
		}

		/// <inheritdoc/>
		public FbxRootNode Parse()
		{
			IFbxParser parser = null;

			if (FbxBinary.ReadHeader(_stream))
			{
				parser = new FbxBinaryParser(_stream, _errorLevel);
			}
			else
			{
				parser = new FbxAsciiParser(_stream, _errorLevel);
			}

			return parser.Parse();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_stream.Dispose();
		}

		/// <summary>
		/// Read fbx file.
		/// </summary>
		/// <returns></returns>
		public static Scene Read(string path, ErrorLevel errorLevel, NotificationHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				reader.OnNotification = notificationHandler;
				return reader.Read();
			}
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public static FbxRootNode Parse(string path, ErrorLevel errorLevel, NotificationHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				reader.OnNotification = notificationHandler;
				return reader.Parse();
			}
		}

		/// <summary>
		/// Read fbx file.
		/// </summary>
		/// <returns></returns>
		public static Scene Read(Stream stream, ErrorLevel errorLevel, NotificationHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				reader.OnNotification = notificationHandler;
				return reader.Read();
			}
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public static FbxRootNode Parse(Stream stream, ErrorLevel errorLevel, NotificationHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				reader.OnNotification = notificationHandler;
				return reader.Parse();
			}
		}
	}
}
