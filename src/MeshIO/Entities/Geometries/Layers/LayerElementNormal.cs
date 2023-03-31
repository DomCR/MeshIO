using CSMath;
using System;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementNormal : LayerElement
	{
		public List<XYZ> Normals { get; set; } = new List<XYZ>();

		public List<double> Weights { get; set; } = new List<double>();

		public LayerElementNormal() : base() { }

		public LayerElementNormal(Geometry owner) : base(owner) { }

		public void Add(XYZ normal, double defaulWheight = 0)
		{
			Normals.Add(normal);
			Weights.Add(defaulWheight);
		}

		public void CalculateFlatNormals()
		{
			if (!(this.Owner is Mesh mesh))
				throw new InvalidOperationException();

			this.Normals.Clear();

			this.MappingMode = MappingMode.ByPolygon;
			this.ReferenceMode = ReferenceMode.Direct;

			foreach (Polygon item in mesh.Polygons)
			{
				XYZ normal = XYZ.FindNormal(
					mesh.Vertices[item.ToArray()[0]],
					mesh.Vertices[item.ToArray()[1]],
					mesh.Vertices[item.ToArray()[2]]);

				this.Normals.Add(normal.Normalize());
			}
		}
	}
}
