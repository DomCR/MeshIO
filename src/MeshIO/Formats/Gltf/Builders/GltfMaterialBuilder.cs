using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Shaders;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfMaterialBuilder : GltfObjectBuilder<GltfMaterial>
{
	public Material Material { get; private set; }

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);
	}
}
