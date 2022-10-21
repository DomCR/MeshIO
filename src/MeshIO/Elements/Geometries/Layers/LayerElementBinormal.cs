﻿using CSMath;
using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementBinormal : LayerElement
	{
		public List<XYZ> Normals { get; set; } = new List<XYZ>();

		public List<double> Weights { get; set; } = new List<double>();

		public LayerElementBinormal() : base() { }
	}
}
