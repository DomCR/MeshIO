using MeshIO.Core;
using MeshIO.FBX.Converters;
using MeshIO.FBX.Exceptions;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxReader : ReaderBase
	{
		public ErrorLevel ErrorLevel { get; }

		private Stream _stream;

		private FbxRootNode _root;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		/// <param name="errorLevel">When to throw an <see cref="FbxException"/></param>
		public FbxReader(string path, ErrorLevel errorLevel = ErrorLevel.Permissive) : this(new FileStream(path, FileMode.Open), errorLevel) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="errorLevel"></param>
		public FbxReader(Stream stream, ErrorLevel errorLevel = ErrorLevel.Permissive)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this.ErrorLevel = errorLevel;

			this._stream = stream;
		}

		/// <summary>
		/// Get the fbx version of the file
		/// </summary>
		/// <returns></returns>
		public FbxVersion GetVersion()
		{
			_root ??= this.Parse();

			return _root.Version;
		}

		/// <summary>
		/// Read the FBX file
		/// </summary>
		public Scene Read()
		{
			_root ??= this.Parse();
			INodeConverter converter = NodeConverterBase.GetConverter(_root);
			converter.OnNotification += this.onNotificationEvent;

			return converter.ConvertScene();
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public FbxRootNode Parse()
		{
			IFbxParser parser = null;

			if (FbxBinary.ReadHeader(_stream))
			{
				parser = new FbxBinaryParser(_stream, ErrorLevel);
			}
			else
			{
				parser = new FbxAsciiParser(_stream, ErrorLevel);
			}

			return parser.Parse();
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
		public static Scene Read(string path, ErrorLevel errorLevel = ErrorLevel.Permissive, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				reader.OnNotification += notificationHandler;
				return reader.Read();
			}
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public static FbxRootNode Parse(string path, ErrorLevel errorLevel = ErrorLevel.Permissive, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				reader.OnNotification += notificationHandler;
				return reader.Parse();
			}
		}

		/// <summary>
		/// Read a fbx stream into an scene
		/// </summary>
		/// <returns></returns>
		public static Scene Read(Stream stream, ErrorLevel errorLevel = ErrorLevel.Permissive, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				reader.OnNotification += notificationHandler;
				return reader.Read();
			}
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public static FbxRootNode Parse(Stream stream, ErrorLevel errorLevel = ErrorLevel.Permissive, NotificationEventHandler notificationHandler = null)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				reader.OnNotification += notificationHandler;
				return reader.Parse();
			}
		}
	}
}
