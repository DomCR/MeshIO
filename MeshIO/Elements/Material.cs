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

		public Color Color { get; set; }

		public Material() : base() { }

		public Material(string name) : base(name) { }
	}
}
