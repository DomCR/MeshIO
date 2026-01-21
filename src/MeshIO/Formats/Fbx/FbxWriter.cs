using MeshIO.Formats.Fbx.Writers;
using System;
using System.IO;

namespace MeshIO.Formats.Fbx;

/// <summary>
/// Implements a <see cref="FbxWriter"/> for writing fbx files.
/// </summary>
public class FbxWriter : SceneWriter<FbxWriterOptions>
{
	/// <summary>
	/// Version to be used in the fbx file
	/// </summary>
	public FbxVersion Version
	{
		get { return this.Options.Version; }
		set { this.Options.Version = value; }
	}

	/// <inheritdoc/>
	public FbxWriter(string path, Scene scene, FbxWriterOptions options = null, NotificationEventHandler notification = null)
		: base(path, scene, options, notification) { }
	
	/// <inheritdoc/>
	public FbxWriter(Stream stream, Scene scene, FbxWriterOptions options = null, NotificationEventHandler notification = null)
		: base(stream, scene, options, notification)
	{
	}

	/// <summary>
	/// Write a <see cref="MeshIO.Scene"/> into a fbx file
	/// </summary>
	/// <param name="path"></param>
	/// <param name="scene"></param>
	/// <param name="options"></param>
	/// <param name="notification"></param>
	public static void Write(string path, Scene scene, FbxWriterOptions options = null, NotificationEventHandler notification = null)
	{
		using (FbxWriter writer = new FbxWriter(path, scene, options, notification))
		{
			writer.Write();
		}
	}

	/// <summary>
	/// Write a <see cref="MeshIO.Scene"/> into a stream
	/// </summary>
	/// <param name="stream"></param>
	/// <param name="scene"></param>
	/// <param name="options"></param>
	/// <param name="notification"></param>
	public static void Write(Stream stream, Scene scene, FbxWriterOptions options = null, NotificationEventHandler notification = null)
	{
		using (FbxWriter writer = new FbxWriter(stream, scene, options, notification))
		{
			writer.Write();
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		_stream.Dispose();
	}

	/// <summary>
	/// Writes the current content using the configured options.
	/// </summary>
	public override void Write()
	{
		FbxFileWriterBase fwriter = FbxFileWriterBase.Create(this._scene, this.Options);

		FbxRootNode n = fwriter.ToNodeStructure();

		using (IFbxStreamWriter sw = FbxStreamWriterFactory.Create(this.Options, n, this._stream))
		{
			sw.Write();
		}
	}
}