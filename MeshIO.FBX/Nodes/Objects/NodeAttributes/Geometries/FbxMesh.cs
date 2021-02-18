using MeshIO.FBX.Nodes.Objects.NodeAttributes.LayerContainers;
using MeshIO.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	/*
	 Geometry: 1932828315568, "Geometry::", "Mesh" {
		Properties70:  {
			P: "Color", "ColorRGB", "Color", "",0.882352941176471,0.341176470588235,0.56078431372549
		}
		Vertices: *24 {
			a: -23.1837253570557,-32.7949714660645,0,23.1837253570557,-32.7949714660645,0,-23.1837253570557,32.7949714660645,0,23.1837253570557,32.7949714660645,0,-23.1837253570557,-32.7949714660645,25.4236392974854,23.1837253570557,-32.7949714660645,25.4236392974854,-23.1837253570557,32.7949714660645,25.4236392974854,23.1837253570557,32.7949714660645,25.4236392974854
		} 
		PolygonVertexIndex: *36 {
			a: 1,0,-4,0,2,-4,7,6,-5,7,4,-6,5,4,-1,5,0,-2,7,5,-2,7,1,-4,6,7,-4,6,3,-3,4,6,-3,4,2,-1
		} 
		Edges: *18 {
			a: 0,1,2,3,4,6,7,8,10,11,13,14,17,20,23,26,29,32
		} 
		GeometryVersion: 124
	}	 
	 */

	public class FbxMesh : FbxGeometry
	{
		public List<XYZ> Vertices { get; set; } = new List<XYZ>();
		public List<Polygon> Polygons { get; set; } = new List<Polygon>();
		protected override FbxObjectType m_objectType => FbxObjectType.Mesh;

		public FbxMesh() : base()
		{
			Layers[0].AddElement(new FbxLayerElementMaterial());
		}
		public FbxMesh(FbxNode node) : base(node)
		{
			buildVertices(node["Vertices"]?.Value as double[]);
			buildPolygons(node["PolygonVertexIndex"]?.Value as int[]);
		}
		public FbxMesh(Mesh mesh) : this()
		{
			Name = mesh.Name;
			Vertices = new List<XYZ>(mesh.Vertices);
			Polygons = new List<Polygon>(mesh.Polygons);
		}
		//****************************************************************
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			node.Nodes.Add(new FbxNode("Vertices", verticesArray()));
			node.Nodes.Add(new FbxNode("PolygonVertexIndex", polygonsArray()));

			return node;
		}
		//****************************************************************
		private void buildVertices(double[] arr)
		{
			//Check for null value
			if (arr == null)
				return;

			//Reset the list
			Vertices.Clear();

			//Create the vertices
			for (int i = 2; i < arr.Length; i += 3)
			{
				XYZ v = new XYZ(arr[i - 2], arr[i - 1], arr[i]);
				Vertices.Add(v);
			}
		}
		private void buildPolygons(int[] arr)
		{
			if (arr == null)
				return;

			//Check if the arr are faces or quads
			if (arr[2] < 0)
			{
				for (int i = 2; i < arr.Length; i += 3)
				{
					Triangle tmp = new Triangle(
						(uint)arr[i - 2],
						(uint)arr[i - 1],
						//Substract a unit to the last
						(uint)(Math.Abs(arr[i])) - 1);

					//Set the material for this polygon
					//if (ElementMaterial != null)
					//	tmp.MaterialIndex = ElementMaterial.GetMaterialIndex(polygons.Count);

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

					//Set the material for this polygon
					//if (ElementMaterial != null)
					//	tmp.MaterialIndex = ElementMaterial.GetMaterialIndex(polygons.Count);

					Polygons.Add(tmp);
				}
			}
		}
		private double[] verticesArray()
		{
			List<double> arr = new List<double>();

			foreach (XYZ v in Vertices)
			{
				arr.Add(v.X);
				arr.Add(v.Y);
				arr.Add(v.Z);
			}

			return arr.ToArray();
		}
		private int[] polygonsArray()
		{
			List<int> arr = new List<int>();

			//Check if the polygons list is empty
			if (!Polygons.Any())
				return arr.ToArray();

			if (Polygons.First() is Triangle)
			{
				foreach (Triangle t in Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add(-((int)t.Index2 + 1));
				}
			}
			else
			{
				foreach (Quad t in Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add((int)t.Index2);
					arr.Add(-((int)t.Index3 + 1));
				}
			}

			return arr.ToArray();
		}
	}
}
