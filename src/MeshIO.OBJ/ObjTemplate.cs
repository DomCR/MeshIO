using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System;
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

		public List<Polygon> MeshPolygons { get; } = [];

		public List<Polygon> TexturePolygons { get; } = [];

		public List<Polygon> NormalPolygons { get; } = [];

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
				LayerElementNormal normals = new LayerElementNormal(MappingMode.ByPolygonVertex, ReferenceMode.IndexToDirect);
				normals.AddRange(this.Normals);
				mesh.Layers.Add(normals);
			}

			if (UVs.Any())
			{
				LayerElementUV uv = new LayerElementUV(MappingMode.ByPolygonVertex, ReferenceMode.IndexToDirect);
				uv.AddRange(this.UVs.Select(xy => xy.Convert<XY>()));
				mesh.Layers.Add(uv);
			}

			if (MeshPolygons.Any())
			{
				mesh.Polygons.AddRange(MeshPolygons);
			}

			return mesh;
		}
	}
}
