using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementWeight : LayerElement
	{
		public List<double> Weights { get; set; } = new List<double>();

		public LayerElementWeight() : base() { }
	}
}
