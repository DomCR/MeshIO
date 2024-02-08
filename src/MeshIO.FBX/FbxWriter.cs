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
		/// Version to be used in the fbx file
		/// </summary>
		public FbxVersion Version { get { return this.Options.Version; } }

		/// <summary>
		/// Scene to write in the stream
		/// </summary>
		public Scene Scene { get; }

		/// <summary>
		/// Writers fot this writer instance
		/// </summary>
		public FbxWriterOptions Options { get; set; }

		private readonly Stream _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="scene"></param>
		/// <param name="options"></param>
		public FbxWriter(string path, Scene scene, FbxWriterOptions options = null) : this(File.Create(path), scene, options) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="scene"></param>
		/// <param name="options"></param>
		public FbxWriter(Stream stream, Scene scene, FbxWriterOptions options = null)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this.Scene = scene;
			this._stream = stream;
			this.Options = options ?? new FbxWriterOptions();
		}

		/// <summary>
		/// Write a <see cref="MeshIO.Scene"/> into a fbx file
		/// </summary>
		/// <param name="path"></param>
		/// <param name="scene"></param>
		/// <param name="options"></param>
		/// <param name="onNotification"></param>
		public static void Write(string path, Scene scene, FbxWriterOptions options = null, NotificationEventHandler onNotification = null)
		{
			using (FbxWriter writer = new FbxWriter(path, scene, options))
			{
				writer.OnNotification += onNotification;
				writer.Write();
			}
		}

		/// <summary>
		/// Write a <see cref="MeshIO.Scene"/> into a stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="scene"></param>
		/// <param name="options"></param>
		/// <param name="onNotification"></param>
		public static void Write(Stream stream, Scene scene, FbxWriterOptions options = null, NotificationEventHandler onNotification = null)
		{
			using (FbxWriter writer = new FbxWriter(stream, scene, options))
			{
				writer.OnNotification += onNotification;
				writer.Write();
			}
		}

		/// <summary>
		/// Write a <see cref="MeshIO.Scene"/>
		/// </summary>
		public void Write()
		{
			this.Write(this.Options);
		}

		/// <summary>
		/// Write a <see cref="MeshIO.Scene"/>
		/// </summary>
		/// <param name="options">Options to apply during the write operation</param>
		public void Write(FbxWriterOptions options)
		{
			FbxFileWriterBase fwriter = FbxFileWriterBase.Create(this.Scene, options);

			FbxRootNode n = fwriter.ToNodeStructure();

			using (IFbxWriter sw = FbxWriterFactory.Create(this.Options, n, this._stream))
			{
				sw.Write();
			}
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			_stream.Dispose();
		}
	}
}