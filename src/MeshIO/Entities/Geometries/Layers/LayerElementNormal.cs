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

		public LayerElementNormal(MappingMode mappingMode, ReferenceMode referenceMode) : base(mappingMode, referenceMode) { }

		/// <summary>
		/// Add a normal
		/// </summary>
		/// <param name="normal"></param>
		/// <param name="wheight">wheight of the normal added</param>
		public void Add(XYZ normal, double wheight = 0)
		{
			this.Normals.Add(normal);
			this.Weights.Add(wheight);
		}

		/// <summary>
		/// Add a collection of normals
		/// </summary>
		/// <param name="normals"></param>
		/// <param name="wheight">wheight of the normal added</param>
		public void AddRange(IEnumerable<XYZ> normals, double wheight = 0)
		{
			foreach (var normal in normals)
			{
				this.Add(normal, wheight);
			}
		}

		public void CalculateFlatNormals()
		{
			if (this.Owner is not Mesh mesh)
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
