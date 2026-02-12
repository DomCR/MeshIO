namespace MeshIO.Materials;

public abstract class Material : Element3D
{
	public Color AmbientColor { get; set; }

	public double AmbientFactor { get; set; }

	public Color DiffuseColor { get; set; }

	public Color EmissiveColor { get; set; }

	public double EmissiveFactor { get; set; }

	public int? MultiLayer { get; set; }

	public double ShininessExponent { get; set; }

	public double TransparencyFactor { get; set; }

	public Material() : this(string.Empty)
	{
	}

	public Material(string name) : base(name)
	{
	}
}