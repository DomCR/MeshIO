using MeshIO.Formats.Fbx.Readers;
using MeshIO.Formats.Fbx.Readers.Parsers;
using System.IO;

namespace MeshIO.Formats.Fbx
{
	/// <summary>
	/// Provides functionality for reading and parsing FBX files or streams into scene objects.
	/// </summary>
	/// <remarks>
	/// The <see cref="FbxReader"/> supports reading FBX data from both file paths and streams. It exposes options for
	/// controlling the reading process via the Options property. Notifications and warnings encountered during reading can
	/// be handled by subscribing to the OnNotification event or by providing a notification handler.
	/// </remarks>
	public class FbxReader : SceneReader
	{
		/// <summary>
		/// Gets the options used to control the behavior of the FBX reader.
		/// </summary>
		/// <remarks>Use this property to configure how FBX files are read, such as specifying import settings or
		/// handling of specific data types. The options are read-only; to modify them, update the properties of the returned
		/// <see cref="FbxReaderOptions"/> instance.</remarks>
		public FbxReaderOptions Options { get; } = new FbxReaderOptions();

		/// <summary>
		/// Initializes a new instance of the FbxReader class for reading FBX files from the specified path.
		/// </summary>
		/// <param name="path">The file system path to the FBX file to be read. Cannot be null or empty.</param>
		/// <param name="notification">An optional delegate to receive notifications or warnings during the reading process. If null, notifications are not
		/// raised.</param>
		public FbxReader(string path, NotificationEventHandler notification = null)
			: base(path, notification) { }

		/// <summary>
		/// Initializes a new instance of the FbxReader class to read FBX data from the specified stream.
		/// </summary>
		/// <remarks>The caller is responsible for managing the lifetime of the provided stream. The FbxReader does not
		/// close or dispose the stream when finished.</remarks>
		/// <param name="stream">The input stream containing FBX data to be read. The stream must be readable and positioned at the start of the FBX
		/// content.</param>
		/// <param name="notification">An optional event handler for receiving notifications or warnings during the reading process. If null, notifications
		/// are not raised.</param>
		public FbxReader(Stream stream, NotificationEventHandler notification = null)
			: base(stream, notification) { }

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
			using (IFbxParser parser = getParser(this._stream.Stream, this.Options))
			{
				root = parser.Parse();
			}

			return root;
		}

		/// <inheritdoc/>
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