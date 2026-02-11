namespace MeshIO.Shaders;

public class PhongMaterial : Material
{
	public Color ReflectionColor { get; set; }
	public double ReflectionFactor { get; set; }
	public double Shininess { get; set; }
	public Color SpecularColor { get; set; }

	public double SpecularFactor { get; set; }
	public PhongMaterial() : this(string.Empty)
	{
	}

	public PhongMaterial(string name) : base(name)
	{
	}
}
