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

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read from</param>
		public FbxReader(string path) : base(File.OpenRead(path)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		public FbxReader(Stream stream) : base(stream) { }

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

		/// <summary>
		/// Parse the FBX file
		/// </summary>
		public FbxRootNode Parse()
		{
			FbxRootNode root;
			using (IFbxParser parser = getParser(this._stream, this.Options))
			{
				root = parser.Parse();
			}

			return root;
		}

		/// <summary>
		/// Read the FBX file
		/// </summary>
		public override Scene Read()
		{
			FbxRootNode root = this.Parse();
			var reader = FbxFileBuilderBase.Create(root, this.Options);
			reader.OnNotification += this.onNotificationEvent;

			return reader.Read();
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
