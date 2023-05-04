using CSMath;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementTangent : LayerElement //TODO: inherit LayerElementWeight ???
	{
		public List<XYZ> Tangents { get; set; } = new List<XYZ>();

		public List<double> Weights { get; set; } = new List<double>();
	}
}
