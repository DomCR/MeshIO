using CSUtilities.IO;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V2;
using System.IO;

namespace MeshIO.Formats.Gltf;

public class GltfReader : SceneReader<GltfReaderOptions>
{
	private readonly StreamIO _bin;

	public GltfReader(string path, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(path, options, notification)
	{
		var binFile = Path.ChangeExtension(path, ".bin");
		this._bin = new StreamIO(File.Open(binFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
	}

	public GltfReader(Stream gltf, Stream gltfBinary, GltfReaderOptions options = null, NotificationEventHandler notification = null)
		: base(gltf, options, notification)
	{
		this._bin = new StreamIO(gltfBinary);
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
