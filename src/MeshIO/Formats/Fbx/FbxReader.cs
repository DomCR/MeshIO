using MeshIO.Formats.Fbx.Parsers;
using MeshIO.Formats.Fbx.Readers;
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
	public class FbxReader : SceneReader<FbxReaderOptions>
	{
		/// <inheritdoc/>
		public FbxReader(string path, FbxReaderOptions options = null, NotificationEventHandler notification = null)
			: base(path, options, notification) { }

		/// <inheritdoc/>
		public FbxReader(Stream stream, FbxReaderOptions options = null, NotificationEventHandler notification = null)
			: base(stream, options, notification) { }

		/// <summary>
		/// Determines the content type of the data in the specified stream.
		/// </summary>
		/// <remarks>The method does not consume or modify the contents of the stream. After the method returns, the
		/// stream position is reset to the beginning. The caller is responsible for managing the lifetime of the
		/// stream.</remarks>
		/// <param name="stream">The input stream containing the data to analyze. The stream must be readable and seekable. The position of the
		/// stream will be reset to the beginning before and after the operation.</param>
		/// <returns>A value of type <see cref="MeshIO.Formats.ContentType"/> that indicates the detected content type of the stream.</returns>
		public static MeshIO.Formats.ContentType GetContentType(Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			var content = FbxBinary.ReadHeader(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return content;
		}

		/// <summary>
		/// Reads an FBX file from the specified path and returns a parsed scene object.
		/// </summary>
		/// <param name="path">The file system path to the FBX file to read. Must refer to a valid FBX file.</param>
		/// <param name="options">Optional reader options that control how the FBX file is parsed. If null, default options are used.</param>
		/// <param name="notification">An optional event handler for receiving notifications or warnings during the read operation. If null, notifications
		/// are ignored.</param>
		/// <returns>A Scene object representing the contents of the FBX file. Returns null if the file cannot be read or parsed.</returns>
		public static Scene Read(string path, FbxReaderOptions options = null, NotificationEventHandler notification = null)
		{
			using (FbxReader reader = new FbxReader(path, options, notification))
			{
				return reader.Read();
			}
		}

		/// <summary>
		/// Reads an FBX scene from the specified stream using the provided reader options and notification handler.
		/// </summary>
		/// <remarks>The caller is responsible for ensuring that the stream remains open and accessible for the duration
		/// of the read operation. This method does not modify the position of the stream after reading.</remarks>
		/// <param name="stream">The input stream containing the FBX data to be read. The stream must be readable and positioned at the start of the
		/// FBX content.</param>
		/// <param name="options">Optional reader options that configure how the FBX data is parsed. If null, default options are used.</param>
		/// <param name="notification">An optional event handler for receiving notifications during the reading process. If null, no notifications are
		/// raised.</param>
		/// <returns>A Scene object representing the contents of the FBX file. Returns null if the stream does not contain a valid FBX
		/// scene.</returns>
		public static Scene Read(Stream stream, FbxReaderOptions options = null, NotificationEventHandler notification = null)
		{
			using (FbxReader reader = new FbxReader(stream, options, notification))
			{
				return reader.Read();
			}
		}

		/// <summary>
		/// Parses the input stream and returns the root node of the FBX document.
		/// </summary>
		/// <returns>The root node of the parsed FBX document. The returned object represents the top-level structure of the FBX file.</returns>
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
			var builder = FbxFileBuilderBase.Create(root, this.Options);
			builder.OnNotification += this.onNotificationEvent;

			return builder.Build();
		}

		private static IFbxParser getParser(Stream stream, FbxReaderOptions options)
		{
			IFbxParser parser = null;
			if (FbxBinary.ReadHeader(stream) == ContentType.Binary)
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