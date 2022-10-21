using MeshIO.GLTF.Schema.V2;

namespace MeshIO.GLTF
{
	internal class GltfBinaryReaderV2 : GltfBinaryReaderBase
	{
		public GltfBinaryReaderV2(GltfRoot root, byte[] chunk) : base(root, chunk) { }
	}
}
