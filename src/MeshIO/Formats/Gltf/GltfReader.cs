using CSUtilities.IO;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V2;
using System.IO;

namespace MeshIO.Formats.Gltf;

/// <summary>
/// Provides functionality to read and parse glTF (GL Transmission Format) files and streams into scene objects.
/// </summary>
/// <remarks>
/// The GltfReader supports reading standard glTF files. It can be
/// constructed from a file path or from streams, and allows for custom reader options and notification handling.
/// </remarks>
public class GltfReader : SceneReader<GltfReaderOptions>
{
	private readonly StreamIO _bin;

	/// <summary>
	/// Initializes a new instance of the GltfReader class to read a GLTF file and its associated binary data.
	/// </summary>
	/// <remarks>The constructor automatically locates and opens the binary file associated with the specified GLTF
	/// file. The reader supports concurrent access to the binary file if it is being used by other processes.</remarks>
	/// <param name="path">The path to the GLTF file to be read. Must not be null or empty.</param>
	/// <param name="options">Optional settings that control the behavior of the reader. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications during the reading process. If null, no notifications are
	/// sent.</param>
	public GltfReader(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(path, options, notification)
	{
		var binFile = Path.ChangeExtension(path, ".bin");
		this._bin = new StreamIO(File.Open(binFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
	}

	/// <summary>
	/// Initializes a new instance of the GltfReader class for reading glTF assets from the specified JSON and binary
	/// streams.
	/// </summary>
	/// <remarks>The caller is responsible for managing the lifetime of the provided streams. Both streams must
	/// remain open for the duration of the GltfReader instance.</remarks>
	/// <param name="gltf">A stream containing the glTF JSON content. The stream must be readable and positioned at the start of the glTF
	/// data.</param>
	/// <param name="gltfBinary">A stream containing the binary buffer data associated with the glTF asset. The stream must be readable and
	/// positioned at the start of the binary data.</param>
	/// <param name="options">Optional settings that control how the glTF asset is read and processed. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications or warnings during the reading process. If null, no
	/// notifications are raised.</param>
	public GltfReader(Stream gltf, Stream gltfBinary, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(gltf, options, notification)
	{
		this._bin = new StreamIO(gltfBinary);
	}

	/// <summary>
	/// Reads a glTF file from the specified path and returns the parsed scene.
	/// </summary>
	/// <remarks>This method provides a convenient way to load a glTF file from disk in a single call. The caller is
	/// responsible for ensuring that the file exists and is accessible. If the file is invalid or cannot be parsed, an
	/// exception may be thrown.</remarks>
	/// <param name="path">The file system path to the glTF file to read. Must not be null or empty.</param>
	/// <param name="options">Optional settings that control how the glTF file is read and parsed. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications or warnings during the reading process. If null,
	/// notifications are ignored.</param>
	/// <returns>A Scene object representing the contents of the glTF file.</returns>
	public static Scene Read(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
	{
		using (GltfReader reader = new GltfReader(path, options, notification))
		{
			return reader.Read();
		}
	}

	/// <summary>
	/// Reads a glTF scene from the specified stream and optional binary buffer, using the provided options and
	/// notification handler.
	/// </summary>
	/// <remarks>The caller is responsible for managing the lifetime of the input streams. This method does not
	/// close or dispose the provided streams.</remarks>
	/// <param name="stream">The stream containing the glTF JSON content to read. The stream must be readable and positioned at the start of the
	/// glTF data.</param>
	/// <param name="gltfBinary">An optional stream containing the binary buffer referenced by the glTF file, or null if the glTF does not use an
	/// external binary buffer. The stream must be readable and positioned at the start of the binary data.</param>
	/// <param name="options">Optional settings that control how the glTF is read and processed. If null, default options are used.</param>
	/// <param name="notification">An optional event handler for receiving notifications or warnings encountered during reading. If null,
	/// notifications are ignored.</param>
	/// <returns>A Scene object representing the parsed glTF scene. Returns null if the scene could not be read or is invalid.</returns>
	public static Scene Read(Stream stream, Stream gltfBinary, GltfReaderOptions options = null, NotificationEventHandler notification = null)
	{
		using (GltfReader reader = new GltfReader(stream, gltfBinary, options, notification))
		{
			return reader.Read();
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();
		this._bin.Dispose();
	}

	/// <inheritdoc/>
	public override Scene Read()
	{
		GltfHeader header = GltfHeader.Read(this._stream, this._bin);
		GltfRoot root = header.GetRoot();
		var reader = new GlbFileBuilder(header);
		reader.OnNotification += this.onNotificationEvent;
		return reader.Build();
	}
}