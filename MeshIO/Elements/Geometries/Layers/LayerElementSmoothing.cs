using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementSmoothing : LayerElement
	{
		public List<int> Smoothing { get; set; } = new List<int>();

		public LayerElementSmoothing() : base() { }

		public LayerElementSmoothing(Geometry owner) : base(owner) { }
	}
}
