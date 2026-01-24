using MeshIO.Formats.Gltf.Readers;

namespace MeshIO.Formats.Gltf.Builders;

internal interface IGltfObjectBuilder
{
	public bool HasBeenBuilt { get; }

	public void Build(GlbV2FileBuilder builder);
}
