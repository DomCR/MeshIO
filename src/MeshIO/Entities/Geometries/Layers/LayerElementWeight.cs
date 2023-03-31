﻿using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementWeight : LayerElement
	{
		public List<double> Weights { get; set; } = new List<double>();

		public LayerElementWeight() : base() { }

		public LayerElementWeight(Geometry owner) : base(owner) { }
	}
}
