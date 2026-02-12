namespace MeshIO.Formats.Gltf.Schema.V2;

public interface IGltfTextureInfo
{
	public string Index { get; }

	public float Scale { get; }

	public int TexCoord { get; }
}