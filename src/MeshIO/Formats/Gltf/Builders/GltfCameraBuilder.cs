using MeshIO.Entities;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfCameraBuilder : GltfObjectBuilder<GltfCamera>
{
	public Camera Camera { get; private set; }

	public override void Build(GlbFileBuilder builder)
	{
		base.Build(builder);

		this.Camera = new Camera();
	}
}