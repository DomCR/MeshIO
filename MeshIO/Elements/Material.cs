using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements
{
	public class Material : Element
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

		public Material() : base() { }

		public Material(string name) : base(name) { }
	}
}
