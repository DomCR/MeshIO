using MeshIO.Entities.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.FBX.Readers.Templates
{
	internal class FbxMeshTemplate : FbxGeometryTemplate<Mesh>
	{
		public FbxMeshTemplate(FbxNode node) : base(node, new Mesh())
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			this.readPolygons();
			this.readEdges();
		}

		private void readEdges()
		{
			if (this.FbxNode.TryGetNode("Edges", out FbxNode edges))
			{
				this.Element.Edges.AddRange(this.toArr<int>(edges.Value as IEnumerable));
			}
		}

		private void readPolygons()
		{
			if (this.FbxNode.TryGetNode("PolygonVertexIndex", out FbxNode polygons))
			{
				this.Element.Polygons = this.mapPolygons(polygons.Value as int[]);
			}
		}

		protected List<Polygon> mapPolygons(int[] arr)
		{
			List<Polygon> Polygons = new List<Polygon>();

			if (arr == null)
				return Polygons;

			//Check if the arr are faces or quads
			if (arr[2] < 0)
			{
				for (int i = 2; i < arr.Length; i += 3)
				{
					Triangle tmp = new Triangle(
						arr[i - 2],
						arr[i - 1],
						//Substract a unit to the last
						(Math.Abs(arr[i])) - 1);

					Polygons.Add(tmp);
				}
			}
			//Quads
			else if (arr[3] < 0)
			{
				for (int i = 3; i < arr.Length; i += 4)
				{
					Quad tmp = new Quad(
						Math.Abs(arr[i - 3]),
						Math.Abs(arr[i - 2]),
						Math.Abs(arr[i - 1]),
						//Substract a unit to the last
						Math.Abs(arr[i]) - 1);

					Polygons.Add(tmp);
				}
			}

			return Polygons;
		}
	}
}
