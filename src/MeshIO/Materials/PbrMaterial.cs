namespace MeshIO.Materials;

public class PbrMaterial : Material
{
	public Texture EmissiveTexture { get; set; }

	public Texture NormalTexture { get; set; }

	public Texture OcclusionTexture { get; set; }

	public PbrMaterial() : this(string.Empty)
	{
	}

	public PbrMaterial(string name) : base(name)
	{
	}
}