using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema;
using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Materials;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfTextureBuilder : GltfObjectBuilder<GltfTexture>
{
	public Texture Texture { get; private set; }

	public GltfTextureBuilder()
	{ }

	public override void Build(GlbFileBuilder builder)
	{
		this.Texture = new Texture(this.GltfObject.Name);

		base.Build(builder);

		if (builder.Samplers.TryGetValue(this.GltfObject.Sampler, out var sampler))
		{
			this.Texture.MagnificationFilter = ((int?)sampler.MagFilter).Convert();
			this.Texture.MinificationFilter = ((int?)sampler.MinFilter).Convert(out TextureFilterType mipFilter);
			this.Texture.MipFilter = mipFilter;

			this.Texture.WrapModeT = ((int)sampler.WrapT).Convert();
			this.Texture.WrapModeS = ((int)sampler.WrapS).Convert();
		}

		if (builder.Images.TryGetValue(this.GltfObject.Source, out var image))
		{
		}
	}
}