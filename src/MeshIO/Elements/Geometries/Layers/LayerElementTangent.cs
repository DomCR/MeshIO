using CSMath;
using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementTangent : LayerElement //TODO: inherit LayerElementWeight ???
	{
		public List<XYZ> Tangents { get; set; } = new List<XYZ>();

		public List<double> Weights { get; set; } = new List<double>();

		public LayerElementTangent() : base() { }

		public LayerElementTangent(Geometry owner) : base(owner) { }
	}
}
