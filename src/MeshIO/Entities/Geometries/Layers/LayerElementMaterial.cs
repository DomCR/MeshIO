using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementMaterial : LayerElement
	{
		public LayerElementMaterial() : base() { }

		public LayerElementMaterial(Geometry owner) : base(owner)
		{
			// this.Indices.Add(0);
		}
	}
}
