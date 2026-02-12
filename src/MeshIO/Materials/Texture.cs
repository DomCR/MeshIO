namespace MeshIO.Materials;

public enum TextureFilterType
{
	None = 0,

	Nearest = 1,

	Linear = 2,
}

public enum WrapMode
{
	Clamp = 0,
	Repeat = 1,
	MirroredRepeat = 2,
}

public class Texture : Element3D
{
	public TextureFilterType MagnificationFilter { get; set; }
	public TextureFilterType MinificationFilter { get; set; }
	public TextureFilterType MipFilter { get; set; }
	public WrapMode WrapModeT {  get; set; }
	public WrapMode WrapModeS {  get; set; }
	public Texture() : base()
	{
	}

	public Texture(string name) : base(name)
	{
	}
}
