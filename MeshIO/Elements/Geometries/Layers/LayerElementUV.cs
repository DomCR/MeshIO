using CSMath;
using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementUV : LayerElement
	{
		public List<XY> UV { get; set; } = new List<XY>();

		public LayerElementUV(Geometry owner) : base(owner) { }
	}
}
