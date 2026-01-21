using MeshIO.Formats.Gltf.Schema.V2;

namespace MeshIO.Formats.Gltf.Readers;

internal class GltfBinaryReaderV2 : GltfBinaryReaderBase
{
	public GltfBinaryReaderV2(GltfRoot root, byte[] chunk) : base(root, chunk) { }
}
