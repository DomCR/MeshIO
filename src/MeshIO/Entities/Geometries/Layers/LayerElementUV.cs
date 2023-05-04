using CSMath;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementUV : LayerElement
	{
		public List<XY> UV { get; set; } = new List<XY>();

		public LayerElementUV() : base() { }
			}
}
