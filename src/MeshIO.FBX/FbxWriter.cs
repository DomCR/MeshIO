using MeshIO.Core;
using MeshIO.FBX.Writers;
using System;
using System.IO;

namespace MeshIO.FBX
{
	/// <summary>
	/// Implements a <see cref="FbxWriter"/> for writing fbx files.
	/// </summary>
	public class FbxWriter : WriterBase
	{
		/// <summary>
		/// Version to be used in the fbx file.
		/// </summary>
		public FbxVersion Version { get; }

		/// <summary>
		/// Scene to write in the stream
		/// </summary>
		public Scene Scene { get; }

		/// <summary>
		/// Root node to write in the stream
		/// </summary>
		/// <remarks>
		/// This node will be generated before the file is writen
		/// </remarks>
		public FbxRootNode RootNode { get; private set; }

		private readonly Stream _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="scene"></param>
		/// <param name="version"></param>
		public FbxWriter(string path, Scene scene, FbxVersion version = FbxVersion.v7400) : this(File.Create(path), scene, version) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="scene"></param>
		/// <param name="version"></param>
		public FbxWriter(Stream stream, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this.Scene = scene;
			this.Version = version;
			this._stream = stream;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="rootNode"></param>
		/// <param name="version"></param>
		public FbxWriter(string path, FbxRootNode rootNode, FbxVersion version = FbxVersion.v7400) : this(File.Create(path), rootNode, version) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="rootNode"></param>
		/// <param name="version"></param>
		public FbxWriter(Stream stream, FbxRootNode rootNode, FbxVersion version = FbxVersion.v7400)
		{
			this.Version = version;
			this.RootNode = rootNode;
			this._stream = stream;
		}

		public static void Write(Stream stream, Scene scene, FbxVersion version = FbxVersion.v7700, FbxFileFormat fileFormat = FbxFileFormat.Binary, NotificationEventHandler onNotification = null)
		{
			var root = FbxRootNode.CreateFromScene(scene, version);
			Write(stream, root, version, fileFormat, onNotification);
		}

		public static void Write(Stream stream, FbxRootNode root, FbxVersion version = FbxVersion.v7700, FbxFileFormat fileFormat = FbxFileFormat.Binary, NotificationEventHandler onNotification = null)
		{
			using (FbxWriter writer = new FbxWriter(stream, root, version))
			{
				writer.OnNotification += onNotification;
				writer.Write();
			}
		}

		/// <summary>
		/// Write a <see cref="MeshIO.Scene"/> into a fbx an ascii file.
		/// </summary>
		/// <param name="fileFormat"></param>
		public void Write(FbxFileFormat fileFormat = FbxFileFormat.Binary)
		{
			if (this.RootNode == null)
			{
				this.RootNode = FbxRootNode.CreateFromScene(this.Scene, this.Version);
			}

			using (IFbxWriter writer = FbxWriterBase.GetWriter(this.RootNode, this._stream, fileFormat))
			{
				writer.Write();
			}
		}

		public void Write(FbxWriterOptions options)
		{
			using (var writer = FbxFileWriterBase.Create(this.Scene, options, this._stream))
			{
				writer.Write();
			}
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			_stream.Dispose();
		}
	}
}