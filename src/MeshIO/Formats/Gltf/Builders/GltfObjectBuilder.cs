using MeshIO.Formats.Gltf.Readers;

namespace MeshIO.Formats.Gltf.Builders;

internal abstract class GltfObjectBuilder<R> : IGltfObjectBuilder
{
	public R GltfObject { get; set; }

	public bool HasBeenBuilt { get; private set; } = false;

	public GltfObjectBuilder()
	{ }

	public GltfObjectBuilder(R gltfObject)
	{
		GltfObject = gltfObject;
	}

	public virtual void Build(GlbV2FileBuilder builder)
	{
		HasBeenBuilt = true;
	}
}
