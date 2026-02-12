using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Materials;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfMaterialBuilder : GltfObjectBuilder<GltfMaterial>
{
	public Material Material { get; private set; }

	public override void Build(GlbFileBuilder builder)
	{
		this.Material = new PbrMaterial(this.GltfObject.Name);
		var pbrMat = this.Material as PbrMaterial;

		base.Build(builder);

		if (this.GltfObject.PbrMetallicRoughness != null)
		{
		}

		if (this.GltfObject.NormalTexture != null
			&& builder.TryGetBuilder(this.GltfObject.NormalTexture.Index,
			out GltfTextureBuilder normalBuilder))
		{
			pbrMat.NormalTexture = normalBuilder.Texture;
		}

		if (this.GltfObject.OcclusionTexture != null
			&& builder.TryGetBuilder(this.GltfObject.NormalTexture.Index,
			out GltfTextureBuilder occlusionBuilder))
		{
			pbrMat.OcclusionTexture = occlusionBuilder.Texture;
		}

		if (this.GltfObject.EmissiveTexture != null 
			&& builder.TryGetBuilder(this.GltfObject.NormalTexture.Index,
			out GltfTextureBuilder emissiveBuilder))
		{
			pbrMat.EmissiveTexture = emissiveBuilder.Texture;
		}
	}

	private Texture processTexture(GlbFileBuilder builder)
	{
		throw new System.NotImplementedException();
	}
}
