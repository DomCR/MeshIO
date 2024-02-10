using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.OBJ
{
	internal class ObjTemplate
	{
		public string Name { get; set; }

		public List<XYZM> Vertices { get; } = [];

		public List<XYZ> Normals { get; } = [];

		public List<XYZ> UVs { get; } = [];

		public ObjTemplate(string name)
		{
			this.Name = name;
		}

		public Mesh CreateMesh()
		{
			Mesh mesh = new Mesh();

			mesh.Vertices.AddRange(Vertices.Select(v => v.Convert<XYZ>()));

			if (Normals.Any())
			{
				LayerElementNormal normals = new LayerElementNormal();
			}

			return mesh;
		}
	}
}
