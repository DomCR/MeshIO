namespace MeshIO.Shaders
{
	/// <summary>
	/// Base material class
	/// </summary>
	/// <remarks>
	/// This class is a temporary placeholder to gather the material information
	/// </remarks>
	public class Material : Element3D
	{
		public string ShadingModel { get; set; } = "unknown";

		public int? MultiLayer { get; set; }

		public Color AmbientColor { get; set; }

		public Color DiffuseColor { get; set; }

		public Color SpecularColor { get; set; }

		public double SpecularFactor { get; set; }

		public double ShininessExponent { get; set; }

		public double TransparencyFactor { get; set; }

		public Color EmissiveColor { get; set; }

		public double EmissiveFactor { get; set; }

		public Material() : this(string.Empty) { }

		public Material(string name) : base(name) { }
	}

	public class PbrMaterial : Material
	{
		public PbrMaterial() : this(string.Empty) { }

		public PbrMaterial(string name) : base(name) { }
	}
}
