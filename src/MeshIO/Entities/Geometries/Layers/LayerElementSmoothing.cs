using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementSmoothing : LayerElement
	{
		public List<int> Smoothing { get; set; } = new List<int>();

		public LayerElementSmoothing() : base() { }
	}
}
