using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema;
using System;
using System.IO;

namespace MeshIO.Formats.Gltf;

/// <summary>
/// Provides functionality for reading 3D scenes from GLB (GL Transmission Format Binary) files.
/// </summary>
/// <remarks>The GlbReader class enables loading and parsing of GLB files, which are binary containers for glTF
/// assets. It supports reading from both file paths and streams, and can be used to obtain a Scene object representing
/// the contents of the GLB file. Notification handlers can be attached to receive progress or warning messages during
/// the reading process.</remarks>
public class GlbReader : SceneReader<GltfReaderOptions>
{
	private GlbHeader _header;

	/// <inheritdoc/>
	public GlbReader(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(path, options, notification) { }

	/// <inheritdoc/>
	public GlbReader(Stream stream, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(stream, options, notification)
	{
	}

	/// <summary>
	/// Reads a glTF or GLB file from the specified path and returns the parsed scene.
	/// </summary>
	/// <param name="path">The file system path to the glTF or GLB file to read. Cannot be null or empty.</param>
	/// <param name="options">Optional settings that control how the file is read and parsed. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications or warnings during the reading process. If null,
	/// notifications are ignored.</param>
	/// <returns>A Scene object representing the contents of the glTF or GLB file.</returns>
	public static Scene Read(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
	{
		using (GlbReader reader = new GlbReader(path, options, notification))
		{
			return reader.Read();
		}
	}

	/// <summary>
	/// Reads a glTF scene from the specified stream using the provided options and notification handler.
	/// </summary>
	/// <param name="stream">The input stream containing the glTF or GLB data to read. The stream must be readable and positioned at the start
	/// of the glTF or GLB content.</param>
	/// <param name="options">An optional set of reader options that control parsing behavior. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications or warnings during the reading process. If null,
	/// notifications are ignored.</param>
	/// <returns>A Scene object representing the parsed glTF scene from the input stream.</returns>
	public static Scene Read(Stream stream, GltfReaderOptions options = null, NotificationEventHandler notification = null)
	{
		using (GlbReader reader = new GlbReader(stream, options, notification))
		{
			return reader.Read();
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();
	}

	/// <inheritdoc/>
	public override Scene Read()
	{
		GlbHeader header = GlbHeader.Read(_stream.Stream);

		IGlbFileBuilder reader;
		switch (header.Version)
		{
			case GltfVersion.V1:
			case GltfVersion.V2:
				reader = new GlbFileBuilder(header);
				break;
			default:
				throw new NotSupportedException($"Version {this._header.Version} not supported.");
		}

		reader.OnNotification += this.onNotificationEvent;
		return reader.Build();
	}
}