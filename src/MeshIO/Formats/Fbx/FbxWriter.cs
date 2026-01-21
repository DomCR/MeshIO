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

	public FbxWriter(string path, Scene scene, FbxWriterOptions options = null, NotificationEventHandler notification = null)
		: base(path, scene, options, notification) { }

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

	/// <summary>
	/// Write a <see cref="MeshIO.Scene"/>
	/// </summary>
	public override void Write()
	{
		this.Write(this.Options);
	}

	/// <summary>
	/// Write a <see cref="MeshIO.Scene"/>
	/// </summary>
	/// <param name="options">Options to apply during the write operation</param>
	public void Write(FbxWriterOptions options)
	{
		FbxFileWriterBase fwriter = FbxFileWriterBase.Create(this._scene, options);

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