using MeshIO.FBX.Nodes.Objects.NodeAttributes.Geometries.Layers;
using MeshIO.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes
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
		LayerElementNormal: 0 {
			Version: 102
			Name: ""
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Normals: *108 {
				a: 0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0
			} 
			NormalsW: *36 {
				a: 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
			} 
		}
		LayerElementBinormal: 0 {
			Version: 102
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Binormals: *108 {
				a: 0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1
			} 
			BinormalsW: *36 {
				a: 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
			} 

		}
		LayerElementTangent: 0 {
			Version: 102
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Tangents: *108 {
				a: -1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0
			} 
			TangentsW: *36 {
				a: 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
			} 
		}
		LayerElementUV: 0 {
			Version: 101
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "IndexToDirect"
			UV: *48 {
				a: 1,0,0,0,1,1,0,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1
			} 
			UVIndex: *36 {
				a: 1,0,3,0,2,3,7,6,4,7,4,5,11,10,8,11,8,9,15,14,12,15,12,13,19,18,16,19,16,17,23,22,20,23,20,21
			} 
		}
		LayerElementSmoothing: 0 {
			Version: 102
			Name: ""
			MappingInformationType: "ByPolygon"
			ReferenceInformationType: "Direct"
			Smoothing: *12 {
				a: 2,2,4,4,8,8,16,16,32,32,64,64
			} 
		}
		LayerElementMaterial: 0 {
			Version: 101
			Name: ""
			MappingInformationType: "AllSame"
			ReferenceInformationType: "IndexToDirect"
			Materials: *1 {
				a: 0
			} 
		}
		Layer: 0 {
			Version: 100
			LayerElement:  {
				Type: "LayerElementNormal"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementBinormal"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementTangent"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementMaterial"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementSmoothing"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementUV"
				TypedIndex: 0
			}
		}
	}	 
	 */

	public class FbxMesh : FbxGeometry
	{
		public List<XYZ> Vertices { get; set; } = new List<XYZ>();
		public List<Polygon> Polygons { get; set; } = new List<Polygon>();
		public FbxLayerContainer Layers { get; set; }
		[Obsolete("Only to save info while implementing the class")]
		private FbxNode node;
		protected override FbxObjectType m_objectType => FbxObjectType.Mesh;

		public FbxMesh(FbxNode node) : base(node)
		{
			this.node = node;

			buildVertices(node["Vertices"]?.Value as double[]);
			buildPolygons(node["PolygonVertexIndex"]?.Value as int[]);
		}

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
						(Math.Abs(arr[i])) - 1);

					//Set the material for this polygon
					//if (ElementMaterial != null)
					//	tmp.MaterialIndex = ElementMaterial.GetMaterialIndex(polygons.Count);

					Polygons.Add(tmp);
				}
			}
		}
	}
}
